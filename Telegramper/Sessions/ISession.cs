namespace Telegramper.Sessions
{
    public interface ISession
    {
        Task<T?> GetAndRemoveAsync<T>(long? entityId = null, string? key = null) where T : class;
        Task<T?> GetAsync<T>(long? entityId = null, string? key = null) where T : class;
        Task<T> GetAsync<T>(T defaultValue, long? entityId = null, string? key = null) where T : class;
        Task SetAsync<T>(T value, long? entityId = null, string? key = null) where T : class;
        Task SetAsync<T>(Action<T> changeValues, long? entityId = null, string? key = null) where T : class;
        Task SetAsync<T>(T defaultValue, Action<T> changeValues, long? entityId = null, string? key = null) where T : class;
        Task RemoveAsync<T>(long? entityId = null, string? key = null) where T : class;
    }
}
