using Telegram.Framework.Session.Storage.Saver;
using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.Session.Storage
{
    public class SessionDataStorage : ISessionDataStorage
    {
        private readonly ISessionDataSaver _saver;
        private readonly UpdateContext _updateContext;

        public SessionDataStorage(ISessionDataSaver saver, UpdateContextAccessor accessor)
        {
            _saver = saver;
            _updateContext = accessor.UpdateContext;
        }

        public async Task SetAsync<T>(string key, T data)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(data);

            var userId = _updateContext.TelegramUserId;
            if (userId == null)
                throw new InvalidOperationException("User id is null");

            await _saver.SaveAsync(userId.Value, key, data);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            
            var userId = _updateContext.TelegramUserId;
            if (userId == null)
                throw new InvalidOperationException("User id is null");

            return await _saver.LoadAsync<T>(userId.Value, key);
        }

        public async Task RemoveAsync(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            
            var userId = _updateContext.TelegramUserId;
            if (userId == null)
                throw new InvalidOperationException("User id is null");

            await _saver.RemoveAsync(userId.Value, key);
        }

        public async Task<T?> GetAndRemoveAsync<T>(string key)
        {
            var data = await GetAsync<T>(key);
            await RemoveAsync(key);
            return data;
        }
    }
}
