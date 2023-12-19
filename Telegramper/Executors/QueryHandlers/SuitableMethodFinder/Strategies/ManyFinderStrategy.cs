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

    public IEnumerable<ExecutorMethod> Find(IEnumerable<RouteMethod> methodsInHandlerQueue, IEnumerable<RouteMethod> methodsWithIgnoreQueueAttribute)
    {
        var methods = methodsInHandlerQueue.Concat(methodsWithIgnoreQueueAttribute);
        
        return methods.Where(method => method
            .TargetAttributes
            .Any(attr => attr.IsTarget(_updateContext.Update))
        ).Select(m => m.Method);
    }
}