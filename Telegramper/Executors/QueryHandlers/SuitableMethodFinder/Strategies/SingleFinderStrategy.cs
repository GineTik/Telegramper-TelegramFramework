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

    public IEnumerable<ExecutorMethod> Find(IEnumerable<RouteMethod> methodsInHandlerQueue, IEnumerable<RouteMethod> methodsWithIgnoreQueueAttribute)
    {
        var suitableMethodInQueue = methodsInHandlerQueue.FirstOrDefault(m => m
            .TargetAttributes
            .Any(attr => attr.IsTarget(_updateContext.Update))
        );
        
        var suitableMethodsWithIgnoreAttribute = methodsWithIgnoreQueueAttribute.Where(method => method
            .TargetAttributes
            .Any(attr => attr.IsTarget(_updateContext.Update))
        );

        return (suitableMethodInQueue == null
            ? suitableMethodsWithIgnoreAttribute
            : new[] { suitableMethodInQueue }.Concat(suitableMethodsWithIgnoreAttribute)
        ).Select(m => m.Method);
    }
}