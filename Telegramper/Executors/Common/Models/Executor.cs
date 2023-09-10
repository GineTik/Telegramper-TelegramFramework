using Telegramper.Core.Context;
using Telegramper.Core.AdvancedBotClient;
using Microsoft.Extensions.DependencyInjection;
using Telegramper.Executors.QueryHandlers.Factory;

namespace Telegramper.Executors.Common.Models
{
    public abstract class Executor
    {
        public UpdateContext UpdateContext => _updateContext;
        public IAdvancedTelegramBotClient Client => UpdateContext.Client;
        public IServiceProvider ServiceProvider { get; private set; } = default!;

        private IExecutorFactory _factory = default!;
        private UpdateContext _updateContext = default!;

        public void Init(UpdateContext updateContext, IServiceProvider provider)
        {
            ArgumentNullException.ThrowIfNull(updateContext);
            ArgumentNullException.ThrowIfNull(provider);

            ServiceProvider = provider;
            _updateContext = updateContext;
            _factory = ServiceProvider.GetRequiredService<IExecutorFactory>();
        }

        public TResult ExecuteAsync<TExecutor, TResult>(Func<TExecutor, TResult> executeMethod)
            where TExecutor : Executor
        {
            ArgumentNullException.ThrowIfNull(executeMethod);

            var executor = _factory.CreateExecutor<TExecutor>();
            return executeMethod.Invoke(executor);
        }

        public async Task ExecuteAsync<TExecutor>(Func<TExecutor, Task> executeMethod)
           where TExecutor : Executor
        {
            await ExecuteAsync<TExecutor, Task>(executeMethod);
        }
    }
}
