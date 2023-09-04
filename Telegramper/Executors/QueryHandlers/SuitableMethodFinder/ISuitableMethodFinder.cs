using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder
{
    public interface ISuitableMethodFinder
    {
        Task<IEnumerable<ExecutorMethod>> FindForCurrentUpdateAsync();
    }
}