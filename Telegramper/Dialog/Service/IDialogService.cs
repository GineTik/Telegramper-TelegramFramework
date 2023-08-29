namespace Telegramper.Dialog.Instance
{
    public interface IDialogService
    {
        Task StartAsync(string dialogName);
        Task NextAsync();
        Task EndAsync();
        Task<bool> IsLaunchedAsync();
    }
}
