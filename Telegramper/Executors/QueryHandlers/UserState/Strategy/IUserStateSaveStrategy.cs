namespace Telegramper.Executors.QueryHandlers.UserState.Strategy
{
    public interface IUserStateSaveStrategy
    {
        Task AddOrUpdateAsync(long userId, IEnumerable<string> states);
        Task<IEnumerable<string>?> GetAsync(long userId);
        Task RemoveAsync(long userId);
    }
}
