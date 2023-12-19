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

    public IEnumerable<ExecutorMethod> Find(IEnumerable<RouteMethod> methodsInHandlerQueue, IEnumerable<RouteMethod> methodsWithIgnoreQueueAttribute)
    {
        var i = 0;
        var suitableMethodsInQueue = new List<ExecutorMethod>();
        
        foreach (var method in methodsInHandlerQueue)
        {
            var targetAttributes = method.TargetAttributes;
            if (!targetAttributes.Any(attr => attr.IsTarget(_updateContext.Update))) continue;

            suitableMethodsInQueue.Add(method.Method);
            i++;

            if (i == _handlerQueueOptions.LimitOfHandlersPerRequest)
                break;
        }

        var suitableMethodsWithIgnoreAttribute = methodsWithIgnoreQueueAttribute.Where(method => method
            .TargetAttributes
            .Any(attr => attr.IsTarget(_updateContext.Update))
        ).Select(m => m.Method);

        return suitableMethodsInQueue.Concat(suitableMethodsWithIgnoreAttribute);
    }
}