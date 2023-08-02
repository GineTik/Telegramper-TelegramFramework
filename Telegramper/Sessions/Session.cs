using Telegramper.Sessions.Saver;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Sessions
{
    public abstract class Session : ISession
    {
        private readonly ISessionDataSaver _sessionDataSaver;
        private readonly UpdateContext _updateContext;

        public Session(
            ISessionDataSaver saver,
            UpdateContextAccessor updateContextAccessor)
        {
            _sessionDataSaver = saver;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        #region Get
        public async Task<T?> GetAndRemoveAsync<T>(long? entityId = null, string? key = null)
        {
            var value = await GetAsync<T>(entityId, key);
            await RemoveAsync<T>(entityId, key);
            return value;
        }

        public async Task<T?> GetAsync<T>(long? entityId = null, string? key = null)
        {
            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= buildKey<T>();

            return await _sessionDataSaver.GetAsync<T>(entityId!.Value, key!);
        }

        public async Task<T> GetAsync<T>(T defaultValue, long? entityId = null, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(defaultValue);

            var value = await GetAsync<T>(entityId, key);
            return value ?? defaultValue;
        }
        #endregion

        #region Set
        public async Task SetAsync<T>(T value, long? entityId = null, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(value);

            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= buildKey<T>();

            await _sessionDataSaver.SetAsync(entityId!.Value, key!, value);
        }

        public async Task SetAsync<T>(Action<T?> changeValues, long? entityId = null, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(changeValues);

            var value = await GetAsync<T>(entityId, key)
                ?? throw new NullReferenceException($"GetAsync returned null. If you are not sure that the item exists in the session, you can use SetAsync with the defaultValue parameter");

            changeValues(value);
            await SetAsync(value, entityId, key);
        }

        public async Task SetAsync<T>(T defaultValue, Action<T> changeValues, long? entityId = null, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(changeValues);

            var value = await GetAsync(defaultValue, entityId, key);
            changeValues(value);
            await SetAsync(value, entityId, key);
        }
        #endregion

        #region Remove
        public async Task RemoveAsync<T>(long? entityId = null, string? key = null)
        {
            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= buildKey<T>();

            await _sessionDataSaver.RemoveAsync(entityId!.Value, key!);
        }
        #endregion

        protected abstract long GetCurrentEntityId(UpdateContext updateContext);

        private static string? buildKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
