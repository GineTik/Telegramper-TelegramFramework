using Telegramper.Sessions.Saver;
using Telegramper.Core.Context;

namespace Telegramper.Sessions
{
    public abstract class Session : ISession
    {
        private readonly ISessionSaveStrategy _sessionSaveStrategy;
        private readonly UpdateContext _updateContext;

        public Session(
            ISessionSaveStrategy saver,
            UpdateContextAccessor updateContextAccessor)
        {
            _sessionSaveStrategy = saver;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        #region Get
        public async Task<T?> GetAndRemoveAsync<T>(long? entityId = null, string? key = null)
            where T : class
        {
            var value = await GetAsync<T>(entityId, key);
            await RemoveAsync<T>(entityId, key);
            return value;
        }

        public async Task<T?> GetAsync<T>(long? entityId = null, string? key = null)
            where T : class
        {
            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= BuildKey<T>();

            return await _sessionSaveStrategy.GetAsync<T>(entityId!.Value, key!);
        }

        public async Task<T> GetAsync<T>(T defaultValue, long? entityId = null, string? key = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(defaultValue);

            var value = await GetAsync<T>(entityId, key);
            return value ?? defaultValue;
        }
        #endregion

        #region Set
        public async Task SetAsync<T>(T value, long? entityId = null, string? key = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(value);

            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= BuildKey<T>();

            await _sessionSaveStrategy.SetAsync(entityId!.Value, key!, value);
        }

        public async Task SetAsync<T>(Action<T> changeValues, long? entityId = null, string? key = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(changeValues);

            var value = await GetAsync<T>(entityId, key)
                ?? throw new NullReferenceException($"GetAsync returned null. If you are not sure that the item exists in the session, you can use SetAsync with the defaultValue parameter");

            changeValues(value);
            await SetAsync(value, entityId, key);
        }

        public async Task SetAsync<T>(T defaultValue, Action<T> changeValues, long? entityId = null, string? key = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(changeValues);

            var value = await GetAsync(defaultValue, entityId, key);
            changeValues(value);
            await SetAsync(value, entityId, key);
        }
        #endregion

        #region Remove
        public async Task RemoveAsync<T>(long? entityId = null, string? key = null)
            where T : class
        {
            entityId ??= GetCurrentEntityId(_updateContext);
            key ??= BuildKey<T>();

            await _sessionSaveStrategy.RemoveAsync(entityId!.Value, key!);
        }
        #endregion

        protected abstract long GetCurrentEntityId(UpdateContext updateContext);
        protected abstract string? BuildKey<T>();
    }
}
