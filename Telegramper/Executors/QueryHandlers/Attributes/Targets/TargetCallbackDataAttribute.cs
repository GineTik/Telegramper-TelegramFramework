using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
{
    [TargetUpdateType(UpdateType.CallbackQuery)]
    public class TargetCallbackDataAttribute : TargetAttribute
    {
        private const char SeparatorBetweenNameAndParameters = ' ';
        
        private string? _callbackData;

        public TargetCallbackDataAttribute(string? callbacksData = null)
        {
            _callbackData = callbacksData;
        }

        public override bool IsTarget(Update update)
        {
            var dataWithParameters = update.CallbackQuery!.Data;
            if (dataWithParameters == null)
            {
                return false;
            }

            var targetCallbackData = dataWithParameters.Split(SeparatorBetweenNameAndParameters).First();
            return _callbackData == targetCallbackData;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            _callbackData ??= MethodName;
        }
    }
}
