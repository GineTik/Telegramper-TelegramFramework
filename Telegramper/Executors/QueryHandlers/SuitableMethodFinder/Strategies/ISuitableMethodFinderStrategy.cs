using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;

public interface ISuitableMethodFinderStrategy
{
    IEnumerable<Route> Find(IEnumerable<Route> routesInHandlerQueue,
        IEnumerable<Route> routesWithIgnoreQueueAttribute);
}