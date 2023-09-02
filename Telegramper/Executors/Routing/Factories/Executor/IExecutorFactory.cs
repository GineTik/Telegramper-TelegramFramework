namespace Telegramper.Executors.Routing.Factories.Executors
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type);
        T CreateExecutor<T>() where T : Executor;
    }
}
