using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;

public class SingleFinderStrategy : ISuitableMethodFinderStrategy
{
    private readonly UpdateContext _updateContext;

    public SingleFinderStrategy(UpdateContextAccessor updateContextAccessor)
    {
        _updateContext = updateContextAccessor.UpdateContext;
    }

    public IEnumerable<Route> Find(IEnumerable<Route> routesInHandlerQueue, IEnumerable<Route> routesWithIgnoreQueueAttribute)
    {
        var suitableRouteInQueue = routesInHandlerQueue.FirstOrDefault(route => route.TargetAttribute.IsTarget(_updateContext.Update));
        var suitableRoutesWithIgnoreAttribute = routesWithIgnoreQueueAttribute.Where(route => route.TargetAttribute.IsTarget(_updateContext.Update));

        return suitableRouteInQueue == null
            ? suitableRoutesWithIgnoreAttribute
            : new[] { suitableRouteInQueue }.Concat(suitableRoutesWithIgnoreAttribute);
    }
}