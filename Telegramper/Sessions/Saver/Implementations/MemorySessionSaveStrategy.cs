namespace Telegramper.Sessions.Saver.Implementations
{
    public class MemorySessionSaveStrategy : ISessionSaveStrategy
    {
        private readonly Dictionary<string, object> _sessions;
        private readonly object _locker;

        public MemorySessionSaveStrategy()
        {
            _sessions = new();
            _locker = new object();
        }

        public async Task RemoveAsync(long userId, string key)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    _sessions.Remove(buildKey(userId, key));
                }
            });
        }

        public async Task<T?> GetAsync<T>(long userId, string key)
            where T : class
        {
            return await Task.Run(new Func<T?>(() =>
            {
                object? value = null;

                lock (_locker)
                {
                    _sessions.TryGetValue(buildKey(userId, key), out value);
                }

                if (value is T convertedValue)
                {
                    return convertedValue;
                }

                return default;
            }));
        }

        public async Task SetAsync<T>(long userId, string key, T data) 
            where T : class
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    if (_sessions.ContainsKey(buildKey(userId, key)) == true)
                    {
                        _sessions[buildKey(userId, key)] = data!;
                    }
                    else
                    {
                        _sessions.Add(buildKey(userId, key), data!);
                    }
                }
            });
        }

        private string buildKey(long userId, string key)
        {
            return $"{userId}:{key}";
        }
    }
}
