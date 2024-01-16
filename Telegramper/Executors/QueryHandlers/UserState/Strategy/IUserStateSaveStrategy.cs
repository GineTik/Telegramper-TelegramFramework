namespace Telegramper.Executors.QueryHandlers.UserState.Strategy
{
    public interface IUserStateSaveStrategy
    {
        Task AddRangeAsync(long userId, IEnumerable<string> states);
        Task SetRangeAsync(long userId, IEnumerable<string> states);
        Task<IEnumerable<string>?> GetAsync(long userId);
        Task RemoveAllAsync(long userId);
        Task RemoveAsync(long userId, string state);
    }
}
