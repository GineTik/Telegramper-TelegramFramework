using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class RouteStorageInitializer : IDictionaryStorageInitializer<RouteTree>
    {
        private readonly UserStateOptions _userStateOptions;
        private readonly IEnumerable<ExecutorMethod> _methods;

        public RouteStorageInitializer(
            IOptions<UserStateOptions> userStateOptions,
            IListStorage<ExecutorMethod> executorMethodStorage)
        {
            _userStateOptions = userStateOptions.Value;
            _methods = executorMethodStorage.Items;
        }

        public RouteTree Initialization()
        {
            return new RouteTree(_userStateOptions.DefaultUserState, _methods);
        }
    }
}
