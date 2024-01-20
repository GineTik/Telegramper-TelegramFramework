namespace Telegramper.Sequence
{
    public static class StaticSequenceUserStateFactory
    {
        public static string Create(string sequenceName)
        {
            return SequenceConstants.ModificatorForName + SequenceConstants.Separator + 
                   sequenceName;
        }

        public static string CreateByIndex(string sequenceName, int index)
        {
            return SequenceConstants.ModificatorForStepIndex + SequenceConstants.Separator + 
                   sequenceName + SequenceConstants.Separator + 
                   index;
        }
    }
}
