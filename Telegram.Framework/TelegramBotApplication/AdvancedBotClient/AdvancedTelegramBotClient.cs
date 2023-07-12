using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Bot;

namespace Telegram.Framework.TelegramBotApplication.AdvancedBotClient
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
