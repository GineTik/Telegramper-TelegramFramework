using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegramper.Executors.Build.Options;
using Telegramper.Executors.Building;
using Telegramper.Executors.Building.Command;
using Telegramper.Executors.Building.NameTransformer;
using Telegramper.Executors.Routing;
using Telegramper.Executors.Routing.Factories.Executors;
using Telegramper.Executors.Routing.ParametersParser;
using Telegramper.Executors.Routing.Preparer;
using Telegramper.Executors.Routing.RoutesStorage;
using Telegramper.Executors.Routing.UserState;
using Telegramper.Executors.Routing.UserState.Saver;

namespace Telegramper.Executors.Build.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Action<ExecutorOptions>? configure = null)
        {
            return services.AddExecutors(null, configure);
        }

        public static IServiceCollection AddExecutors(this IServiceCollection services,
            IEnumerable<Assembly>? assemblies = null, Action<ExecutorOptions>? configureAction = null)
        {
            assemblies ??= new[] { Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()! }.Where(a => a != null);

            var executorTypes = ExecutorFinder.FindExecutorTypes(assemblies);
            var executorOptions = configureOptions(services, configureAction, executorTypes);
            services.addExecutorServices(executorOptions, executorTypes);

            return services;
        }

        private static ExecutorOptions configureOptions(
            IServiceCollection services,
            Action<ExecutorOptions>? configure,
            IEnumerable<Type> executorTypes)
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

            services.Configure<FindedExecutorOptinons>(options =>
            {
                options.ExecutorTypes = executorTypes;
            });

            return executorOptions;
        }

        private static void addExecutorServices(
            this IServiceCollection services,
            ExecutorOptions executorOptions,
            IEnumerable<Type> executorTypes)
        {
            services.AddTransient(typeof(IParametersParser), executorOptions.ParameterParser.ParserType);
            services.AddTransient<IExecutorMethodInvoker, ExecutorMethodInvoker>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<IUserStates, UserStates>();
            services.AddTransient<ISuitableMethodFinder, SuitableMethodFinder>();
            services.AddTransient<IExecutorMethodPreparer, ExecutorMethodPreparer>();

            services.AddSingleton(typeof(INameTransformer), executorOptions.MethodNameTransformer.Type);
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);
            services.AddSingleton<IRoutesStorage, RoutesStorage>();
            services.AddSingleton<ICommandStorage, ExecutorCommandStorage>();

            foreach (var executor in executorTypes)
                services.AddTransient(executor);
        }
    }
}
