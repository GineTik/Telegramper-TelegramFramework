using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Telegramper.Dialog.Attributes
{
    [TargetUpdateType(UpdateType.Unknown)]
    public class TargetDialogStepAttribute : TargetAttribute
    {
        public string DialogName { get; private set; } = default!;
        
        public string? Key { get; set; }
        public int Priority { get; set; }
        public int Index { get; private set; }

        public string? Question { get; }
        public ParseMode? ParseMode { get; set; }

        public override string? UserStates => StaticDialogUserStateFactory.CreateByIndex(DialogName, Index);

        public TargetDialogStepAttribute(string? question)
        {
            Question = question;
        }

        public override bool IsTarget(Update update)
        {
            return true;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            DialogName = 
                method.ExecutorType.GetCustomAttribute<DialogNameAttribute>()?.DialogName
                ?? method.ExecutorType.Name;

            Index = method.ExecutorType
                .GetMethods()
                .Where(method => method.GetCustomAttribute<TargetDialogStepAttribute>() != null)
                .OrderByDescending(method => method.GetCustomAttribute<TargetDialogStepAttribute>()!.Priority)
                .ToList()
                .IndexOf(method.MethodInfo);
        }
    }
}
