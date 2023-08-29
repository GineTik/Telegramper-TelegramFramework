namespace Telegramper.Dialog.Storages
{
    public interface IDialogStepStorage
    {
        IDictionary<string, ICollection<DialogStep>> Steps { get; }
    }
}
