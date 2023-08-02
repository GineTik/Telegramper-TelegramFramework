using Telegramper.Executors.Configuration.Options;
using Telegramper.Executors.Routing.Storage.RouteDictionaries;
using Telegramper.Executors.Routing.Storage.StaticHelpers;

namespace Telegramper.Executors.Routing.Storage
{
    public class StaticRoutesStorageFactory
    {
        public static RoutesStorage Create(IServiceProvider serviceProvider, 
            IEnumerable<Type> executorsTypes, ExecutorOptions executorOptions)
        {
            var methods = ExecutorMethodsHelper.TakeExecutorMethodsFrom(executorsTypes);
            var routes = new UpdateTypeDictionary(serviceProvider, executorOptions.UserState.DefaultUserState);
            routes.AddMethods(methods);

            return new RoutesStorage(methods, routes);
        }
    }
}
