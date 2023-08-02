using System.Reflection;
using Telegramper.Executors.Routing.Storage.RouteDictionaries;

namespace Telegramper.Executors.Routing.Storage
{
    public sealed class RoutesStorage : IRoutesStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }
        public UpdateTypeDictionary Routes { get; }

        public RoutesStorage(IEnumerable<MethodInfo> methods, UpdateTypeDictionary routes)
        {
            Methods = methods;
            Routes = routes;
        }
    }
}
