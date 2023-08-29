namespace Telegramper.Sessions.Saver
{
    public interface ISessionDataSaver
    {
        // entityId is UserId or ChatId
        Task SetAsync<T>(long entityId, string key, T data) where T : class;
        Task<T?> GetAsync<T>(long entityId, string key) where T : class;
        Task RemoveAsync(long entityId, string key);
    }
}
