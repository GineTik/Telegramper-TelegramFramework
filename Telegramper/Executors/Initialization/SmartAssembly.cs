using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Initialization
{
    public class SmartAssembly
    {
        public Assembly Assembly { get; }
        public IEnumerable<FilterAttribute> AssemblyAttributes { get; }

        public SmartAssembly(Assembly assembly, IEnumerable<FilterAttribute>? assemblyAttributes = null)
        {
            Assembly = assembly;
            AssemblyAttributes = assemblyAttributes ?? new List<FilterAttribute>();
        }
    }
}
