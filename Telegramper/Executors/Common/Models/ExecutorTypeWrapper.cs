using System.Reflection;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorTypeWrapper
    {
        public Type Type { get; set; } = default!;
        public IEnumerable<Attribute> Attributes { get; set; } = default!;

        public ExecutorTypeWrapper(Type type, IEnumerable<Attribute> attributes)
        {
            Type = type;
            Attributes = GetCustomAttributes(attributes);
        }

        public IEnumerable<Attribute> GetCustomAttributes(IEnumerable<Attribute> attributes)
        {
            return Type
                .GetCustomAttributes()
                .Concat(attributes);
        }
    }
}
