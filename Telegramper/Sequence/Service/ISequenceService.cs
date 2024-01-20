using Telegramper.Executors.Common.Models;

namespace Telegramper.Sequence.Service
{
    public interface ISequenceService
    {
        Task StartAsync(string sequenceName);
        Task ShiftAsync(int offset = 1);
        Task EndAsync(bool executeEndCallback = true);
        Task<bool> IsLaunchedAsync();
    }
}
