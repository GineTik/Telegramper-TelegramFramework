namespace Telegramper.Sessions
{
    public interface ISession
    {
        Task<T?> GetAndRemoveAsync<T>(long? entityId = null, string? key = null);
        Task<T?> GetAsync<T>(long? entityId = null, string? key = null);
        Task<T> GetAsync<T>(T defaultValue, long? entityId = null, string? key = null);
        Task SetAsync<T>(T value, long? entityId = null, string? key = null);
        Task SetAsync<T>(Action<T?> changeValues, long? entityId = null, string? key = null);
        Task SetAsync<T>(T defaultValue, Action<T> changeValues, long? entityId = null, string? key = null);
        Task RemoveAsync<T>(long? entityId = null, string? key = null);
    }
}
