namespace Telegramper.Dialog.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DialogNameAttribute : Attribute
    {
        public string? DialogName { get; }

        public DialogNameAttribute(string? dialogName)
        {
            DialogName = dialogName;
        }
    }
}
