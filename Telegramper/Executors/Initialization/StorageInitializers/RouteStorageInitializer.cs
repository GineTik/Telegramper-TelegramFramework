using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class RouteStorageInitializer : IDictionaryStorageInitializer<RoutesDictionary>
    {
        private readonly IEnumerable<ExecutorMethod> _methods;

        public RouteStorageInitializer(
            IListStorage<ExecutorMethod> executorMethodStorage)
        {
            _methods = executorMethodStorage.Items;
        }

        public RoutesDictionary Initialization()
        {
            var routes = new RoutesDictionary();
            var allRoutes = _methods.SelectMany(method => method.TargetAttributes.Select(targetAttribute => 
                new Route
                {
                    TargetAttribute = targetAttribute,
                    Method = method,
                }));
            
            foreach (var route in allRoutes)
            {
                foreach (var updateType in route.TargetAttribute.UpdateTypes)
                {
                    foreach (var userState in route.TargetAttribute.UserStates)
                    {
                        routes[updateType].AddOrSet(userState, route);
                    }
                }
            }
            
            return routes;
        }
    }
}
