namespace Telegramper.Session.Storage
{
    public interface ISessionDataStorage
    {
        public Task<T?> GetAsync<T>(string key);
        public Task<T?> GetAndRemoveAsync<T>(string key);
        public Task SetAsync<T>(string key, T data);
        public Task RemoveAsync(string key);
    }
}
