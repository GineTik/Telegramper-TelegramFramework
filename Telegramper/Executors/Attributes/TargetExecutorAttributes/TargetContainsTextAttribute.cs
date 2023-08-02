using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegramper.Executors.Attributes.BaseAttributes;

namespace Telegramper.Executors.Attributes.TargetExecutorAttributes
{
    [TargetUpdateTypes(UpdateType.Message)]
    public class TargetContainsTextAttribute : TargetAttribute
    {
        public string Text { get; set; }

        public TargetContainsTextAttribute(string text)
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
