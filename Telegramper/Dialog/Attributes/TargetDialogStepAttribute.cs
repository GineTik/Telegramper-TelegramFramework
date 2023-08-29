using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors;
using Telegramper.Executors.Routing.Attributes.BaseAttributes;
using Telegramper.Executors.Routing.Attributes.TargetExecutorAttributes;

namespace Telegramper.Dialog.Attributes
{
    [TargetUpdateType(UpdateType.Unknown)]
    public class TargetDialogStepAttribute : TargetAttribute
    {
        public string DialogName { get; set; } = default!;
        public int Priority { get; set; }
        public string Question { get; }

        private string _userState = default!;
        public override string? UserStates => _userState ??= DialogConstants.ModificatorKey + DialogName + ":" + Priority;

        public TargetDialogStepAttribute(string question)
        {
            Question = question;
        }

        public override bool IsTarget(Update update)
        {
            return true;
        }

        protected override void BeforeInitialization(ExecutorMethod method, IServiceProvider serviceProvider)
        {
            DialogName ??= 
                method.ExecutorType.GetCustomAttribute<DialogNameAttribute>()?.DialogName
                ?? method.ExecutorType.Name;

            var sortedStepAttributes = method.ExecutorType
                    .GetMethods()
                    .Where(method => method.GetCustomAttribute<TargetDialogStepAttribute>() != null)
                    .OrderBy(method => method.GetCustomAttribute<TargetDialogStepAttribute>()!.Priority)
                    .ToList();

            var methodIndex = sortedStepAttributes
                .IndexOf(method.MethodInfo);

            Priority = methodIndex;
        }
    }
}
