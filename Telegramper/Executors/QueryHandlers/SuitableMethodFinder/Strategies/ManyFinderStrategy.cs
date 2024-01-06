using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;

public class ManyFinderStrategy : ISuitableMethodFinderStrategy
{
    private readonly UpdateContext _updateContext;

    public ManyFinderStrategy(UpdateContextAccessor updateContextAccessor)
    {
        _updateContext = updateContextAccessor.UpdateContext;
    }

    public IEnumerable<Route> Find(IEnumerable<Route> routesInHandlerQueue, IEnumerable<Route> routesWithIgnoreQueueAttribute)
    {
        var routes = routesInHandlerQueue.Concat(routesWithIgnoreQueueAttribute);
        return routes.Where(route => route.TargetAttribute.IsTarget(_updateContext.Update));
    }
}