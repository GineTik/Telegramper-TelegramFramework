using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.Factory
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type);
        T CreateExecutor<T>() where T : Executor;
    }
}
