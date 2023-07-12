using Telegram.Bot.Types;

namespace Telegram.Framework.Executors.Storages.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }
    }
}
