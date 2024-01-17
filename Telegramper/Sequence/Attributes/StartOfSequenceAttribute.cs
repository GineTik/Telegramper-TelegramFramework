namespace Telegramper.Sequence.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class StartOfSequenceAttribute : Attribute
{
    public string? SequenceName { get; set; }
}