using Telegramper.Executors.Common.Models;

namespace Telegramper.Sequence.Service
{
    public interface ISequenceService
    {
        Task StartAsync(string dialogName);
        Task StartAsync<T>() where T : Executor;
        Task StartAsync(Type sequenceType);
        Task NextAsync();
        Task EndAsync();
        Task<bool> IsLaunchedAsync();
    }
}
