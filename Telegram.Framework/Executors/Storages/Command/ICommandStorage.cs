using Telegram.Framework.Attributes.TargetExecutorAttributes;

namespace Telegram.Framework.Executors.Storages.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<TargetCommandsAttribute> Commands { get; }
    }
}
