using System.Reflection;
using Telegram.Framework.Executors.Routing.Storage.RouteDictionaries;

namespace Telegram.Framework.Executors.Routing.Storage
{
    public interface IRoutesStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }
        public UpdateTypeDictionary Routes { get; }
    }
}
