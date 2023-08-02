namespace Telegramper.Sessions.Saver
{
    public interface ISessionDataSaver
    {
        // entityId is UserId or ChatId
        Task SetAsync<T>(long entityId, string key, T data);
        Task<T?> GetAsync<T>(long entityId, string key);
        Task RemoveAsync(long entityId, string key);
    }
}
