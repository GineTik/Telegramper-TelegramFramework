using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Initialization
{
    public class SmartAssembly
    {

        public Assembly Assembly { get; set; } = default!;
        public IEnumerable<FilterAttribute> GlobalAttributes { get; set; }

        public SmartAssembly(Assembly assembly, IEnumerable<Attribute>? globalAttributes = null)
        {
            Assembly = assembly;
            GlobalAttributes = (IEnumerable<FilterAttribute>)(globalAttributes ?? new List<Attribute>());
        }
    }
}
