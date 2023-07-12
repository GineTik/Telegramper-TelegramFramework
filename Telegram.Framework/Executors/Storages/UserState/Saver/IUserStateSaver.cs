namespace Telegram.Framework.Executors.Storages.UserState.Saver
{
    public interface IUserStateSaver
    {
        Task SaveAsync(long userId, IEnumerable<string> states);
        Task<IEnumerable<string>?> LoadAsync(long userId);
        Task RemoveAsync(long userId);
    }
}
