using System.Reflection;

namespace Telegramper.Executors.Configuration.Options
{
    public class TargetMethodOptinons
    {
        public IEnumerable<Type> ExecutorsTypes { get; set; } = default!;
        public IEnumerable<MethodInfo> MethodInfos { get; set; } = default!;
    }
}
