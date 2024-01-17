namespace Telegramper.Sequence.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SequenceNameAttribute : Attribute
    {
        public string? SequenceName { get; }

        public SequenceNameAttribute(string? sequenceName)
        {
            SequenceName = sequenceName;
        }
    }
}
