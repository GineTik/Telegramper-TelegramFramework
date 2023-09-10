using Telegramper.Core.Context;
using Telegram.Bot;

namespace Telegramper.Core.AdvancedBotClient
{
    public class AdvancedTelegramBotClient : TelegramBotClient, IAdvancedTelegramBotClient
    {
        public UpdateContext UpdateContext { get; }

        public AdvancedTelegramBotClient(string token, UpdateContext updateContext, HttpClient? httpClient = null) : base(token, httpClient)
        {
            UpdateContext = updateContext;
        }
    }
}
