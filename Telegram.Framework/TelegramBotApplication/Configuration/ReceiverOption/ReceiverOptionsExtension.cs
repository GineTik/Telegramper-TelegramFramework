using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Telegram.Framework.TelegramBotApplication.Configuration.ReceiverOption
{
    public static class ReceiverOptionsExtension
    {
        public static ReceiverOptions ConfigureAllowedUpdates(this ReceiverOptions options, params UpdateType[] allowedUpdates)
        {
            ArgumentNullException.ThrowIfNull(allowedUpdates);
            options.AllowedUpdates = allowedUpdates;
            return options;
        }

        public static ReceiverOptions Configure(this ReceiverOptions options, Action<ReceiverOptions> configure)
        {
            configure(options);
            return options;
        }
    }
}
