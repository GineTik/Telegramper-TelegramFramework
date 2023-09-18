using Telegramper.Executors.Common.Models;

namespace Telegramper.Dialog.Service
{
    public interface IDialogService
    {
        Task StartAsync(string dialogName);
        Task StartAsync<T>() where T : Executor;
        Task NextAsync();
        Task EndAsync();
        Task<bool> IsLaunchedAsync();
    }
}
