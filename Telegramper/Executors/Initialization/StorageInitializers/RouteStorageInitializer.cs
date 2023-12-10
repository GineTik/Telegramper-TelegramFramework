using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Initialization.Models;
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
            var temporaryMethodsData = _methods.SelectMany(method => 
                method.TargetAttributes.Select(targetAttribute => new TemporaryMethodDataForInitialization
            {
                TargetAttribute = targetAttribute,
                Method = method,
            }));
            
            foreach (var temporaryMethod in temporaryMethodsData)
            {
                foreach (var updateType in temporaryMethod.TargetAttribute.UpdateTypes)
                {
                    foreach (var userState in temporaryMethod.TargetAttribute.UserStatesAsEnumerable)
                    {
                        routes[updateType].AddOrSet(userState, temporaryMethod);
                    }
                }
            }
            
            return routes;
        }
    }
}
