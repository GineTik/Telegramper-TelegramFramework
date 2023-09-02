using Telegramper.Executors.Routing.RoutesStorage.RouteDictionaries;

namespace Telegramper.Executors.Routing.RoutesStorage
{
    public interface IRoutesStorage
    {
        public IEnumerable<ExecutorMethod> Methods { get; }
        public RouteTree Routes { get; }
    }
}
