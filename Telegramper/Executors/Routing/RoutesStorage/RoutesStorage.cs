using Microsoft.Extensions.Options;
using Telegramper.Executors.Build.Options;
using Telegramper.Executors.Building;
using Telegramper.Executors.Building.NameTransformer;
using Telegramper.Executors.Routing.RoutesStorage.RouteDictionaries;

namespace Telegramper.Executors.Routing.RoutesStorage
{
    public sealed class RoutesStorage : IRoutesStorage
    {
        public IEnumerable<ExecutorMethod> Methods { get; }
        public RouteTree Routes { get; }

        public RoutesStorage(
            IOptions<FindedExecutorOptinons> options,
            IOptions<UserStateOptions> userStateOptions,
            IServiceProvider serviceProvider)
        {
            Methods = ExecutorFinder.FindExecutorsMethods(options.Value.ExecutorTypes, serviceProvider);
            Routes = new RouteTree(userStateOptions.Value.DefaultUserState, Methods);
        }
    }
}
 