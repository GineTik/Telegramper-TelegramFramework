namespace Telegramper.Sequence
{
    public static class SequenceConstants
    {
        public static string ModificatorForName { get; } = "Sequence";
        public static string ModificatorForStepIndex { get; } = ModificatorForName + "StepIndex";
        public static string ModificatorForStepName { get; } = ModificatorForName + "StepName";
        public static string Separator { get; } = ":";
    }
}
