using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Attributes.BaseAttributes;

namespace Telegramper.Executors.Attributes.TargetExecutorAttributes
{
    [TargetUpdateTypes(UpdateType.Message)]
    public class TargetTextAttribute : TargetAttribute
    {
        public string Text { get; set; }

        public TargetTextAttribute(string text)
        {
            Text = text;
        }

        public override bool IsTarget(Update update)
        {
            return Text == update.Message!.Text;
        }
    }
}
