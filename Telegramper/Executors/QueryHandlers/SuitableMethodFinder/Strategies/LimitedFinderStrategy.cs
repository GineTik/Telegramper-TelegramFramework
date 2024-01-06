using Microsoft.Extensions.Options;
using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;

public class LimitedFinderStrategy : ISuitableMethodFinderStrategy
{
    private readonly UpdateContext _updateContext;
    private readonly HandlerQueueOptions _handlerQueueOptions;

    public LimitedFinderStrategy(UpdateContextAccessor updateContextAccessor, IOptions<HandlerQueueOptions> handlerQueueOptions)
    {
        _updateContext = updateContextAccessor.UpdateContext;
        _handlerQueueOptions = handlerQueueOptions.Value;
    }

    public IEnumerable<Route> Find(IEnumerable<Route> routesInHandlerQueue, IEnumerable<Route> routesWithIgnoreQueueAttribute)
    {
        var suitableRoutesInQueue = findSuitableRoutes(routesInHandlerQueue);
        var suitableMethodsWithIgnoreAttribute = routesWithIgnoreQueueAttribute.Where(route => route.TargetAttribute.IsTarget(_updateContext.Update));

        return suitableRoutesInQueue.Concat(suitableMethodsWithIgnoreAttribute);
    }

    private IEnumerable<Route> findSuitableRoutes(IEnumerable<Route> routesInHandlerQueue)
    {
        var routesFound = 0;
        foreach (var route in routesInHandlerQueue)
        {
            if (!route.TargetAttribute.IsTarget(_updateContext.Update)) continue;

            routesFound++;
            yield return route;

            if (routesFound == _handlerQueueOptions.LimitOfHandlersPerRequest)
                break;
        }
    }
}