using Microsoft.Extensions.DependencyInjection;
using Telegramper.Executors.Helpers.Exceptions;
using Telegramper.Core.Context;

namespace Telegramper.Executors.Helpers.Factories.Executors
{
    public class ExecutorFactory : IExecutorFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UpdateContext _updateContext;

        public ExecutorFactory(IServiceProvider serviceProvider, UpdateContextAccessor accessor)
        {
            _serviceProvider = serviceProvider;
            _updateContext = accessor.UpdateContext;
        }

        public Executor CreateExecutor(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);
            InvalidTypeException.ThrowIfNotImplementation<Executor>(type);

            var executor = (Executor)_serviceProvider.GetRequiredService(type);
            executor.Init(_updateContext, _serviceProvider);
            return executor;
        }

        public T CreateExecutor<T>() where T : Executor
        {
            return (T)CreateExecutor(typeof(T));
        }
    }
}
