using Telegram.Bot.Types;
using Telegramper.Attributes.BaseAttributes;

namespace Telegramper.Attributes.TargetExecutorAttributes
{
    public class TargetUserStateAttribute : TargetAttribute
    {
        public TargetUserStateAttribute(string userStates)
        {
            UserStates = userStates;
        }

        public override bool IsTarget(Update update)
        {
            return true;
        }
    }
}
