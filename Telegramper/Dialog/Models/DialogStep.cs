using Telegramper.Dialog.Attributes;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Dialog.Models
{
    public sealed class DialogStep
    {
        public ExecutorMethod Method { get; set; } = default!;
        public TargetDialogStepAttribute StepAttribute { get; set; } = default!;
    }
}
