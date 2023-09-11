namespace Telegramper.Dialog.Service
{
    public interface IDialogService
    {
        Task StartAsync(string dialogName);
        Task NextAsync();
        Task EndAsync();
        Task<bool> IsLaunchedAsync();
    }
}
