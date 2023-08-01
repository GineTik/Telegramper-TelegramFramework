using Telegram.Framework.Attributes.TargetExecutorAttributes;

namespace Telegram.Framework.Executors.Storages.Command
{
    public class ExecutorCommandStorage : ICommandStorage
    {
        public IEnumerable<TargetCommandsAttribute> Commands { get; }

        public ExecutorCommandStorage(
            IEnumerable<TargetCommandsAttribute> commands)
        {
            Commands = commands;
        }
    }
}
