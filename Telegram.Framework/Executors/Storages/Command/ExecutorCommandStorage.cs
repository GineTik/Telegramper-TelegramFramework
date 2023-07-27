using Telegram.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Framework.Executors.Storages.Command.Factory;
using Telegram.Framework.Executors.Routing.Storage;

namespace Telegram.Framework.Executors.Storages.Command
{
    public class ExecutorCommandStorage : ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }

        public ExecutorCommandStorage(IRoutesStorage storage, IBotCommandFactory factory)
        {
            Commands = storage.Methods
                .SelectMany(method => method
                    .GetCustomAttributes<TargetCommandsAttribute>()
                    .Select(attr => factory.CreateBotCommands(method, attr)))
                .SelectMany(commands => commands);
        }
    }
}
