using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Initialization
{
    public class SmartAssembly
    {

        public Assembly Assembly { get; }
        public IEnumerable<FilterAttribute> GlobalAttributes { get; }

        public SmartAssembly(Assembly assembly, IEnumerable<Attribute>? globalAttributes = null)
        {
            Assembly = assembly;
            GlobalAttributes = (IEnumerable<FilterAttribute>)(globalAttributes ?? new List<Attribute>());
        }
    }
}
