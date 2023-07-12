using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
{
    [TargetUpdateType(UpdateType.CallbackQuery)]
    public class TargetCallbacksDatasAttribute : TargetAttribute
    {
        public string[] CallbacksDatas { get; set; }

        public TargetCallbacksDatasAttribute(string callbacksDatas)
        {
            CallbacksDatas = callbacksDatas.Replace(" ", "").Split(",");
        }

        public override bool IsTarget(Update update)
        {
            if (update.CallbackQuery!.Data is not { } data)
                return false;

            var targetData = data.Split(' ').First();
            return CallbacksDatas.Contains(targetData);
        }
    }
}
