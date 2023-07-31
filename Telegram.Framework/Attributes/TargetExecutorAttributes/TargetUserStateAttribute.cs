using Telegram.Bot.Types;
using Telegram.Framework.Attributes.BaseAttributes;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
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
