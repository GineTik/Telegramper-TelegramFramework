using Microsoft.Extensions.DependencyInjection;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.Initialization.StorageInitializers;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;
using Telegramper.Executors.QueryHandlers.Factory;
using Telegramper.Executors.QueryHandlers.MethodInvoker;
using Telegramper.Executors.QueryHandlers.ParametersParser;
using Telegramper.Executors.QueryHandlers.Preparer;
using Telegramper.Executors.QueryHandlers.Preparer.ErrorHandler;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Executors.QueryHandlers.UserState.Saver;
using Telegramper.Storage.Services;

namespace Telegramper.Executors.Initialization.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services, Action<ExecutorOptions>? configureAction = null)
        {
            var executorOptions = configureOptions(services, configureAction);
            services.addExecutorServices(executorOptions);

            return services;
        }

        private static ExecutorOptions configureOptions(
            IServiceCollection services,
            Action<ExecutorOptions>? configure)
        {
            var executorOptions = new ExecutorOptions();
            configure?.Invoke(executorOptions);

            services.Configure<ParameterParserOptions>(options =>
            {
                options.DefaultSeparator = executorOptions.ParameterParser.DefaultSeparator;
                options.ParserType = executorOptions.ParameterParser.ParserType;
                options.ErrorMessages = executorOptions.ParameterParser.ErrorMessages;
            });

            services.Configure<UserStateOptions>(options =>
            {
                options.DefaultUserState = executorOptions.UserState.DefaultUserState;
                options.SaverType = executorOptions.UserState.SaverType;
            });

            return executorOptions;
        }

        private static void addExecutorServices(
            this IServiceCollection services,
            ExecutorOptions executorOptions)
        {
            var executorsTypes = StaticExecutorFinder.FindExecutorTypes(executorOptions.Assemblies);
            services.AddListStorage<ExecutorType>(_ => executorsTypes);
            services.AddListStorage<ExecutorMethod, ExecutorMethodStorageInitializer>();
            services.AddListStorage<TargetCommandAttribute, CommandStorageInitializer>();
            services.AddDictionaryStorage<RouteTree, RouteStorageInitializer>();

            services.AddTransient<IExecutorMethodInvoker, ExecutorMethodInvoker>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<ISuitableMethodFinder, SuitableMethodFinder>();
            services.AddTransient<IExecutorMethodPreparer, ExecutorMethodPreparer>();
            services.AddTransient<IUserStates, UserStates>();
            services.AddTransient<IParseErrorHandler, ParseErrorHandler>();
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);
            services.AddTransient(typeof(IParametersParser), executorOptions.ParameterParser.ParserType);
            services.AddSingleton(typeof(INameTransformer), executorOptions.MethodNameTransformer.Type);

            foreach (var executor in executorsTypes.Select(wrapper => wrapper.Type))
            {
                services.AddTransient(executor);
            }
        }
    }
}
