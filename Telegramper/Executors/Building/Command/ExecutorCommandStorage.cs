using Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Building.Command
{
    public class ExecutorCommandStorage : ICommandStorage
    {
        public IEnumerable<TargetCommandAttribute> Commands { get; }

        public ExecutorCommandStorage(
            IEnumerable<TargetCommandAttribute> commands)
        {
            Commands = commands;
        }
    }
}
