using Telegramper.Executors.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Storages.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<TargetCommandsAttribute> Commands { get; }
    }
}
