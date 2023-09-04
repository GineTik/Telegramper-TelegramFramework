namespace Telegramper.Executors.QueryHandlers.UserState.Saver
{
    public interface IUserStateSaver
    {
        Task AddAsync(long userId, IEnumerable<string> states);
        Task<IEnumerable<string>?> GetAsync(long userId);
        Task RemoveAsync(long userId);
    }
}
