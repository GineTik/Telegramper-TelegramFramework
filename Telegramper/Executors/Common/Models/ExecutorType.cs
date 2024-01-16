using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorType
    {
        public Type Type { get; }
        public IEnumerable<Attribute> Attributes { get; }

        public ExecutorType(Type type, IEnumerable<FilterAttribute> assemblyAttributes)
        {
            Type = type;
            Attributes = Type.GetCustomAttributes().Concat(assemblyAttributes);
        }
    }
}
