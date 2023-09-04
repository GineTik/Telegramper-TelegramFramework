using Telegram.Bot.Types;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
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
