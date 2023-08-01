using Telegramper.Session.Storage;

namespace Telegramper.Session
{
    public class Session<T> : ISession<T>
    {
        private string _key => "SessionData:" + typeof(T).Name;

        private readonly ISessionDataStorage _dataStorage;

        public Session(ISessionDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<T?> GetAndRemoveAsync()
        {
            return await _dataStorage.GetAndRemoveAsync<T>(_key);
        }

        public async Task<T?> GetAsync()
        {
            return await _dataStorage.GetAsync<T>(_key);
        }

        public async Task SetAsync(T value)
        {
            await _dataStorage.SetAsync<T>(_key, value);
        }

        public async Task RemoveAsync()
        {
            await _dataStorage.RemoveAsync(_key);
        }
    }
}
