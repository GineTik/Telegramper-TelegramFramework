namespace Telegramper.Sequence
{
    public static class StaticDialogUserStateFactory
    {
        public static string Create(string dialogName)
        {
            return $"{SequenceConstants.ModificatorForName}{SequenceConstants.Separator}{dialogName}";
        }

        public static string CreateByIndex(int index)
        {
            return $"{SequenceConstants.ModificatorForStepIndex}{SequenceConstants.Separator}{index}";
        }

        public static string CreateByName(string name)
        {
            return $"{SequenceConstants.ModificatorForStepName}{SequenceConstants.Separator}{name}";
        }
    }
}
