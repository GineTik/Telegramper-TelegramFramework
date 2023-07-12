using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Bot;

namespace Telegram.Framework.TelegramBotApplication.AdvancedBotClient
{
    public interface IAdvancedTelegramBotClient : ITelegramBotClient
    {
        public UpdateContext UpdateContext { get; }
    }
}
