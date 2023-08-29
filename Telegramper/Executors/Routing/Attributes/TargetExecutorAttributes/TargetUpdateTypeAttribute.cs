using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Routing.Attributes.BaseAttributes;

namespace Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TargetUpdateTypeAttribute : TargetAttribute
    {
        public UpdateType[] UpdateTypes { get; set; }

        public TargetUpdateTypeAttribute(params UpdateType[] updateTypes)
        {
            UpdateTypes = updateTypes;
        }

        public override bool IsTarget(Update update)
        {
            return UpdateTypes.Contains(update.Type);
        }
    }
}
