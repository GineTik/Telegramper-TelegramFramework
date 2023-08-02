using Telegramper.Attributes.BaseAttributes;
using System.Reflection;

namespace Telegramper.Executors.Routing.Storage.Models
{
    public class TargetMethodInfo
    {
        public MethodInfo MethodInfo { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
