using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegramper.Executors.Attributes.BaseAttributes;
using Telegramper.Executors.Attributes.TargetExecutorAttributes;
using Telegramper.Executors.NameTransformer;
using Telegramper.Executors.Configuration.Options;
using Telegramper.Executors.Helpers.Factories.Executors;
using Telegramper.Executors.Routing;
using Telegramper.Executors.Routing.ParametersParser;
using Telegramper.Executors.Routing.Storage;
using Telegramper.Executors.Routing.Storage.RouteDictionaries;
using Telegramper.Executors.Routing.Storage.StaticHelpers;
using Telegramper.Executors.Storages.Command;
using Telegramper.Executors.Storages.UserState;
using Telegramper.Executors.Storages.UserState.Saver;

namespace Telegramper.Executors.Configuration.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Action<ExecutorOptions>? configure = null)
        {
            return services.AddExecutors(null, configure);
        }

        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Assembly[]? assemblies = null, Action<ExecutorOptions>? configure = null)
        {
            var executorsTypes = getExecutorsTypes(assemblies);
            var executorOptions = configureOptions(services, executorsTypes, configure);

            services.addTransientServices(executorsTypes, executorOptions);
            services.addSingletonServices(executorsTypes, executorOptions);

            return services;
        }

        private static IEnumerable<Type> getExecutorsTypes(
            Assembly[]? assemblies = null)
        {
            assemblies ??= new[] { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var baseExecutorType = typeof(Executor);
            var executorsTypes = assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(type => type != baseExecutorType && baseExecutorType.IsAssignableFrom(type))
            );

            return executorsTypes;
        }

        private static ExecutorOptions configureOptions(
            IServiceCollection services,
            IEnumerable<Type> executorsTypes, 
            Action<ExecutorOptions>? configure = null)
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

            services.Configure<TargetMethodOptinons>(options => 
            {
                options.ExecutorsTypes = executorsTypes;
                options.MethodInfos = ExecutorMethodsHelper.TakeExecutorMethodsFrom(executorsTypes);
            });

            return executorOptions;
        }

        private static void addTransientServices(
            this IServiceCollection services, 
            IEnumerable<Type> executorsTypes,
            ExecutorOptions executorOptions)
        {
            services.AddTransient<IExecutorRouter, ExecutorRouter>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient(typeof(IParametersParser), executorOptions.ParameterParser.ParserType);

            foreach (var type in executorsTypes)
                services.AddTransient(type);
        }

        private static void addSingletonServices(
            this IServiceCollection services, 
            IEnumerable<Type> executorsTypes,
            ExecutorOptions executorOptions)
        {
            services.AddSingleton(typeof(INameTransformer), executorOptions.MethodNameTransformer.Type);

            services.AddSingleton<IUserStateStorage, UserStateStorage>();
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);

            var routesStorage = StaticRoutesStorageFactory.Create(services.BuildServiceProvider(), executorsTypes, executorOptions);
            var commandStorage = StaticExecutorCommandStorageFactory.Create(routesStorage.Methods);
            services.AddSingleton<IRoutesStorage, RoutesStorage>(_ => routesStorage);
            services.AddSingleton<ICommandStorage, ExecutorCommandStorage>(_ => commandStorage);
        }
    }
}
