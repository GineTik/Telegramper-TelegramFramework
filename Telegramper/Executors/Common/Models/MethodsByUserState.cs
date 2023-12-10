using Telegramper.Executors.Initialization.Models;
using Telegramper.Executors.Initialization.StorageInitializers;

namespace Telegramper.Executors.Common.Models;

public class MethodsByUserState
{
    private readonly List<RouteMethod> _methodsInHandlerQueue = new();
    private readonly List<RouteMethod> _methodsWithIgnoreHandlerAttribute = new();

    public IEnumerable<RouteMethod> MethodsInHandlerQueue => _methodsInHandlerQueue;
    public IEnumerable<RouteMethod> MethodsWithIgnoreHandlerAttribute => _methodsWithIgnoreHandlerAttribute;

    public void Add(TemporaryMethodDataForInitialization temporaryMethodData)
    {
        var correctCollection = temporaryMethodData.Method.IsIgnoresLimitOfHandlers
            ? _methodsWithIgnoreHandlerAttribute
            : _methodsInHandlerQueue;
        
        var routeMethod =
            correctCollection.FirstOrDefault(routeMethod =>
                routeMethod.Method.MethodInfo == temporaryMethodData.Method.MethodInfo);
            
        if (routeMethod != null)
        {
            routeMethod.TargetAttributes.Add(temporaryMethodData.TargetAttribute);      
        }
        else
        {
            correctCollection.Add(new RouteMethod
            {
                TargetAttributes = new[] { temporaryMethodData.TargetAttribute },
                Method = temporaryMethodData.Method
            });
        }
    }
}