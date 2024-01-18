using Telegramper.Executors.Common.Models;

namespace Telegramper.Sequence.Service
{
    public interface ISequenceService
    {
        Task StartAsync(string dialogName);
        Task StartAsync<T>() where T : Executor;
        Task StartAsync(Type sequenceType);
        Task NextAsync(int steps = 1);
        Task BackAsync(int steps = 1);
        Task ShiftAsync(int offset = 1);
        Task EndAsync(bool executeEndCallback = true);
        Task<bool> IsLaunchedAsync();
    }
}
