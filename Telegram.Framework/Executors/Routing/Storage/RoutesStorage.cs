using System.Reflection;
using Telegram.Framework.Executors.Routing.Storage.RouteDictionaries;

namespace Telegram.Framework.Executors.Routing.Storage
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
