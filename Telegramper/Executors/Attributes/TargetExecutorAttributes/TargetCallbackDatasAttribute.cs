using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Attributes.BaseAttributes;

namespace Telegramper.Executors.Attributes.TargetExecutorAttributes
{
    [TargetUpdateTypes(UpdateType.CallbackQuery)]
    public class TargetCallbackDatasAttribute : TargetAttribute
    {
        public string[] CallbackDatas { get; set; }

        public TargetCallbackDatasAttribute(string? callbacksDatas = null)
        {
            CallbackDatas = callbacksDatas?.Replace(" ", "").Split(",")
                ?? new string[0];
        }

        public override bool IsTarget(Update update)
        {
            var data = update.CallbackQuery!.Data;
            if (data == null)
            {
                return false;
            }

            var targetData = data.Split(' ').First();
            if (CallbackDatas.Length == 0)
            {
                return targetData == TransformedMethodName;
            }

            return CallbackDatas.Contains(targetData);
        }
    }
}
