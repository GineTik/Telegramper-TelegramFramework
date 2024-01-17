using System.Reflection;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Sequence.Models;

public class Sequence
{
    public string Name { get; set; } = default!;
    public ICollection<SequenceStep> Steps { get; set; } = new List<SequenceStep>();
    public MethodInfo? StartOfSequence { get; set; }
    public MethodInfo? EndOfSequence { get; set; }
}