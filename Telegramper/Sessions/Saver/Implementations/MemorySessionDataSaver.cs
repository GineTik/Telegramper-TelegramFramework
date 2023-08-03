namespace Telegramper.Sessions.Saver.Implementations
{
    public class MemorySessionDataSaver : ISessionDataSaver
    {
        private readonly Dictionary<string, object> _sessions;
        private readonly object _locker;

        public MemorySessionDataSaver()
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
        {
            return await Task.Run(() =>
            {
                object? value;

                lock (_locker)
                {
                    _sessions.TryGetValue(buildKey(userId, key), out value);
                }

                if (value is T convertedValue)
                {
                    return convertedValue;
                }

                return default;
            });
        }

        public async Task SetAsync<T>(long userId, string key, T data)
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
