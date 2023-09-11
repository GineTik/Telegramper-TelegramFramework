using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Telegramper.Dialog.Attributes
{
    [TargetUpdateType(UpdateType.Unknown)]
    public class TargetDialogStepAttribute : TargetAttribute
    {
        public string DialogName => _dialogName;
        public int Priority { get; set; }
        public string Question { get; }
        public ParseMode ParseMode { get; set; } = ParseMode.MarkdownV2;

        private string _dialogName = default!;
        private string _userState = default!;
        public override string? UserStates => _userState ??= "Dialog:" + _dialogName + ":" + Priority;

        public TargetDialogStepAttribute(string question)
        {
            Question = question;
        }

        public override bool IsTarget(Update update)
        {
            return true;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            _dialogName = 
                method.ExecutorType.GetCustomAttribute<DialogNameAttribute>()?.DialogName
                ?? method.ExecutorType.Name;

            Priority = method.ExecutorType
                .GetMethods()
                .Where(method => method.GetCustomAttribute<TargetDialogStepAttribute>() != null)
                .OrderBy(method => method.GetCustomAttribute<TargetDialogStepAttribute>()!.Priority)
                .ToList()
                .IndexOf(method.MethodInfo);
        }
    }
}
