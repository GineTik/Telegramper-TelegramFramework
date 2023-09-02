namespace Telegramper.Executors.Routing
{
    public interface ISuitableMethodFinder
    {
        Task<IEnumerable<ExecutorMethod>> FindSuitableMethodsForCurrentUpdateAsync();
    }
}