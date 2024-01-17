using Telegramper.Executors.Common.Models;
using Telegramper.Sequence.Attributes;

namespace Telegramper.Sequence.Models
{
    public sealed class SequenceStep
    {
        public ExecutorMethod Method { get; set; } = default!;
        public TargetSequenceStepAttribute StepAttribute { get; set; } = default!;
    }
}
