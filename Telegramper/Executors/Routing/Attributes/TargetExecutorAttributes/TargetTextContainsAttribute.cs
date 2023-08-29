using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Routing.Attributes.BaseAttributes;

namespace Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes
{
    [TargetUpdateType(UpdateType.Message)]
    public class TargetTextContainsAttribute : TargetAttribute
    {
        public string Text { get; set; }

        public TargetTextContainsAttribute(string text)
        {
            Text = text;
        }

        public override bool IsTarget(Update update)
        {
            if (update.Message!.Text == null)
            {
                return false;
            }

            return Text.Contains(update.Message!.Text);
        }
    }
}
