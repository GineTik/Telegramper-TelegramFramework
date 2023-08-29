using Telegramper.Dialog.Attributes;
using Telegramper.Executors;

namespace Telegramper.Dialog
{
    public sealed class DialogStep
    {
        public ExecutorMethod Method { get; set; } = default!;
        public TargetDialogStepAttribute StepAttribute { get; set; } = default!;
    }
}
