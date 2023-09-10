using Telegramper.Core.Context;
using Telegram.Bot;

namespace Telegramper.Core.AdvancedBotClient
{
    public interface IAdvancedTelegramBotClient : ITelegramBotClient
    {
        public UpdateContext UpdateContext { get; }
    }
}
