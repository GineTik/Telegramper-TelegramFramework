using Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes;

namespace Telegramper.Executors.Building.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<TargetCommandAttribute> Commands { get; }
    }
}
