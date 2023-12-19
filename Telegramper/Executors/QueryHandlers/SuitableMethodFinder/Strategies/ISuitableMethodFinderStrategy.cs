using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;

public interface ISuitableMethodFinderStrategy
{
    IEnumerable<ExecutorMethod> Find(IEnumerable<RouteMethod> methodsInHandlerQueue,
        IEnumerable<RouteMethod> methodsWithIgnoreQueueAttribute);
}