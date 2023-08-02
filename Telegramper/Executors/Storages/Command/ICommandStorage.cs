using Telegramper.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Storages.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<TargetCommandsAttribute> Commands { get; }
    }
}
