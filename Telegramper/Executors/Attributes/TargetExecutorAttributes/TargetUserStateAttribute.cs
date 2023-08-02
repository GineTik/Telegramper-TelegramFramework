using Telegram.Bot.Types;
using Telegramper.Executors.Attributes.BaseAttributes;

namespace Telegramper.Executors.Attributes.TargetExecutorAttributes
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
