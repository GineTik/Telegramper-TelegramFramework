using Telegram.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;

namespace Telegram.Framework.Executors.Storages.Command.Factory
{
    public interface IBotCommandFactory
    {
        IEnumerable<BotCommand> CreateBotCommands(MethodInfo method, TargetCommandsAttribute attribute);
    }
}
