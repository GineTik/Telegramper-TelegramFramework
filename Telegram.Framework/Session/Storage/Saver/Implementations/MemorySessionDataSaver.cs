namespace Telegramper.Session.Storage.Saver.Implementations
{
    public class MemorySessionDataSaver : ISessionDataSaver
    {
        private readonly Dictionary<string, object> _sessions;

        public MemorySessionDataSaver()
        {
            _sessions = new();
        }

        public async Task RemoveAsync(long userId, string key)
        {
            await Task.Run(() => _sessions.Remove(buildKey(userId, key)));
        }

        public async Task<T?> LoadAsync<T>(long userId, string key)
        {
            return await Task.Run(() =>
            {
                _sessions.TryGetValue(buildKey(userId, key), out var value);

                if (value is T convertedValue)
                {
                    return convertedValue;
                }

                return default;
            });
        }

        public async Task SaveAsync<T>(long userId, string key, T data)
        {
            await Task.Run(() =>
            {
                if (_sessions.ContainsKey(buildKey(userId, key)) == true)
                {
                    _sessions[buildKey(userId, key)] = data!;
                }
                else
                {
                    _sessions.Add(buildKey(userId, key), data!);
                }
            });
        }

        private string buildKey(long userId, string key)
        {
            return $"{userId}:{key}";
        }
    }
}
