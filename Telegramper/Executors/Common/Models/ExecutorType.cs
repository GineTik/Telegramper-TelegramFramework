using System.Reflection;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorType
    {
        public Type Type { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }

        public ExecutorType(Type type, IEnumerable<Attribute> additionalAttributes)
        {
            Type = type;
            Attributes = getCustomAttributes(additionalAttributes);
        }

        private IEnumerable<Attribute> getCustomAttributes(IEnumerable<Attribute> additionalAttributes)
        {
            return Type
                .GetCustomAttributes()
                .Concat(additionalAttributes);
        }
    }
}
