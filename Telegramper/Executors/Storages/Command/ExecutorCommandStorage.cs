using Telegramper.Executors.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Storages.Command
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
