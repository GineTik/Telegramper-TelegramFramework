using Telegramper.TelegramBotApplication.Context;
using Telegram.Bot;

namespace Telegramper.TelegramBotApplication.AdvancedBotClient
{
    public interface IAdvancedTelegramBotClient : ITelegramBotClient
    {
        public UpdateContext UpdateContext { get; }
    }
}
