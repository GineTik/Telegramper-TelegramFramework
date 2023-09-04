using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
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
