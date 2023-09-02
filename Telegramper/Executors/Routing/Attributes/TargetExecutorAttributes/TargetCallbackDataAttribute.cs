using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Routing.Attributes.BaseAttributes;

namespace Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes
{
    [TargetUpdateType(UpdateType.CallbackQuery)]
    public class TargetCallbackDataAttribute : TargetAttribute
    {
        public string[] CallbackDatas { get; set; }

        public TargetCallbackDataAttribute(string? callbacksDatas = null)
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
                return targetData == MethodName;
            }

            return CallbackDatas.Contains(targetData);
        }
    }
}
