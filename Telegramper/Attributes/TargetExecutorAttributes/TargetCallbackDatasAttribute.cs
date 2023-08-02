using Telegramper.Attributes.BaseAttributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegramper.Attributes.TargetExecutorAttributes
{
    [TargetUpdateTypes(UpdateType.CallbackQuery)]
    public class TargetCallbackDatasAttribute : TargetAttribute
    {
        public string[] CallbacksDatas { get; set; }

        public TargetCallbackDatasAttribute(string callbacksDatas)
        {
            CallbacksDatas = callbacksDatas.Replace(" ", "").Split(",");
        }

        public override bool IsTarget(Update update)
        {
            var data = update.CallbackQuery!.Data;
            if (data == null)
                return false;

            var targetData = data.Split(' ').First();
            return CallbacksDatas.Contains(targetData);
        }
    }
}
