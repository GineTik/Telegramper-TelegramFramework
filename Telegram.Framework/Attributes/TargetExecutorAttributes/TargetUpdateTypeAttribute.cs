using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TargetUpdateTypeAttribute : TargetAttribute
    {
        public UpdateType UpdateType { get; set; }

        public TargetUpdateTypeAttribute(UpdateType updateType)
        {
            UpdateType = updateType;
        }

        public override bool IsTarget(Update update)
        {
            return update.Type == UpdateType;
        }
    }
}
