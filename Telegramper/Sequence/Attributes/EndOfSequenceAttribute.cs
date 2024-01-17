namespace Telegramper.Sequence.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class EndOfSequenceAttribute : Attribute
{
    public string? SequenceName { get; set; }
}