namespace Telegram.Framework.Executors.Helpers.Factories.Executors
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type);
        T CreateExecutor<T>() where T : Executor;
    }
}
