using Telegramper.Dialog.Attributes;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Dialog.Models
{
    public sealed class SequenceStep
    {
        public ExecutorMethod Method { get; set; } = default!;
        public TargetSequenceStepAttribute StepAttribute { get; set; } = default!;
    }
}
