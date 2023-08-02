using System.Reflection;
using Telegramper.Executors.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Storages.Command
{
    public static class StaticExecutorCommandStorageFactory
    {
        public static ExecutorCommandStorage Create(IEnumerable<MethodInfo> methods)
        {
            var commandAttributes = methods
                .SelectMany(method => method.GetCustomAttributes<TargetCommandsAttribute>());

            return new ExecutorCommandStorage(commandAttributes);
        }
    }
}
