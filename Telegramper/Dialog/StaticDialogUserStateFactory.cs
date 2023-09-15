namespace Telegramper.Dialog
{
    public static class StaticDialogUserStateFactory
    {
        public static string Create(string dialogName)
        {
            return $"{DialogConstants.Modificator}{dialogName}";
        }

        public static string CreateByIndex(string dialogName, int index)
        {
            return $"{DialogConstants.Modificator}{dialogName}:{index}";
        }

        public static string CreateByKey(string dialogName, string key)
        {
            return $"{DialogConstants.Modificator}{dialogName}:{key}";
        }
    }
}
