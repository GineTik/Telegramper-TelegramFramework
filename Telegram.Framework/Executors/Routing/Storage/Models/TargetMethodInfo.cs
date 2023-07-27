using Telegram.Framework.Attributes.BaseAttributes;
using System.Reflection;

namespace Telegram.Framework.Executors.Routing.Storage.Models
{
    public class TargetMethodInfo
    {
        public MethodInfo MethodInfo { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
