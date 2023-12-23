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
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;
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

            services.Configure<ParametersParserOptions>(options =>
            {
                options.DefaultSeparator = executorOptions.ParametersParser.DefaultSeparator;
                options.ParserType = executorOptions.ParametersParser.ParserType;
                options.ErrorMessages = executorOptions.ParametersParser.ErrorMessages;
            });

            services.Configure<UserStateOptions>(options =>
            {
                options.DefaultUserState = executorOptions.UserState.DefaultUserState;
                options.SaverType = executorOptions.UserState.SaverType;
            });
            
            services.Configure<HandlerQueueOptions>(options =>
            {
                options.LimitOfHandlersPerRequest = executorOptions.HandlerQueue.LimitOfHandlersPerRequest;
            });

            return executorOptions;
        }

        private static void addExecutorServices(
            this IServiceCollection services,
            ExecutorOptions executorOptions)
        {
            var executorsTypes = new ExecutorTypeStorageInitializer(executorOptions.Assemblies).Initialization();
            services.AddListStorage<ExecutorType>(_ => executorsTypes);
            services.AddListStorage<ExecutorMethod, ExecutorMethodStorageInitializer>();
            services.AddListStorage<TargetCommandAttribute, CommandStorageInitializer>();
            services.AddDictionaryStorage<RoutesDictionary, RouteStorageInitializer>();

            services.AddTransient<IExecutorMethodInvoker, ExecutorMethodInvoker>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<ISuitableMethodFinder, SuitableMethodFinder>();
            services.AddTransient<ManyFinderStrategy>();
            services.AddTransient<SingleFinderStrategy>();
            services.AddTransient<LimitedFinderStrategy>();
            services.AddTransient<IExecutorMethodPreparer, ExecutorMethodPreparer>();
            services.AddTransient<IUserStates, UserStates>();
            services.AddTransient<IParseErrorHandler, ParseErrorHandler>();
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);
            services.AddTransient(typeof(IParametersParser), executorOptions.ParametersParser.ParserType);
            services.AddSingleton(typeof(INameTransformer), executorOptions.MethodNameTransformer.NameTransformerType);

            foreach (var executor in executorsTypes.Select(wrapper => wrapper.Type))
            {
                services.AddTransient(executor);
            }
        }
    }
}
