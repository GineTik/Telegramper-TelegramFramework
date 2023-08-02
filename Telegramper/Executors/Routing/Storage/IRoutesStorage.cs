using System.Reflection;
using Telegramper.Executors.Routing.Storage.RouteDictionaries;

namespace Telegramper.Executors.Routing.Storage
{
    public interface IRoutesStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }
        public UpdateTypeDictionary Routes { get; }
    }
}
