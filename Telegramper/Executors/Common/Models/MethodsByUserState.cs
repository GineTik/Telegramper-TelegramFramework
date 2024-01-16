namespace Telegramper.Executors.Common.Models;

public class MethodsByUserState
{
    private readonly List<Route> _routesInHandlerQueue = new();
    private readonly List<Route> _routesWithIgnoreHandlerAttribute = new();

    public IEnumerable<Route> RoutesInHandlerQueue => _routesInHandlerQueue;
    public IEnumerable<Route> RoutesWithIgnoreHandlerAttribute => _routesWithIgnoreHandlerAttribute;

    public void Add(Route route)
    {
        var correctCollection = route.Method.IsIgnoresLimitOfHandlers
            ? _routesWithIgnoreHandlerAttribute
            : _routesInHandlerQueue;
     
        correctCollection.Add(route);
    }
}