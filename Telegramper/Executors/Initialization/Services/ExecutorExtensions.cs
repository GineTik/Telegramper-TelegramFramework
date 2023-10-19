using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Action<ExecutorOptions>? configure = null)
        {
            return services.AddExecutors(null, configure);
        }

        public static IServiceCollection AddExecutors(this IServiceCollection services,
            IEnumerable<SmartAssembly>? assemblies, Action<ExecutorOptions>? configureAction = null)
        {
            if (assemblies == null)
            {
                List<SmartAssembly> listAsseblies = new List<SmartAssembly>();
                listAsseblies.Add(new SmartAssembly(Assembly.GetExecutingAssembly()));

                if (Assembly.GetEntryAssembly() != null)
                {
                    listAsseblies.Add(new SmartAssembly(Assembly.GetEntryAssembly()!));
                }

                assemblies = listAsseblies;
            }

            ExecutorOptions executorOptions = services.configureOptions(configureAction);
            services.addExecutorServices(executorOptions, assemblies);

            return services;
        }

        private static ExecutorOptions configureOptions(
            this IServiceCollection services,
            Action<ExecutorOptions>? configure)
        {
            ExecutorOptions executorOptions = new ExecutorOptions();
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
            ExecutorOptions executorOptions,
            IEnumerable<SmartAssembly> assemblies)
        {
            IEnumerable<ExecutorTypeWrapper> executorsTypes = StaticExecutorFinder.FindExecutorTypes(assemblies);
            services.AddListStorage<ExecutorTypeWrapper>(_ => executorsTypes);
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

            foreach (Type executor in executorsTypes.Select(wrapper => wrapper.Type))
            {
                services.AddTransient(executor);
            }
        }
    }
}
