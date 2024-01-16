namespace Telegramper.Executors.QueryHandlers.UserState.Strategy
{
    public class MemoryUserStateSaveStrategy : IUserStateSaveStrategy
    {
        private readonly Dictionary<long, IEnumerable<string>> _usersStates = new();
        private readonly object _locker = new();

        public async Task<IEnumerable<string>?> GetAsync(long userId)
        {
            return await Task.Run(() =>
            {
                lock (_locker)
                {
                    _usersStates.TryGetValue(userId, out var userState);
                    return userState;
                }
            });
        }

        public async Task RemoveAsync(long userId)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    _usersStates.Remove(userId);
                }
            });
        }

        public async Task AddOrUpdateAsync(long userId, IEnumerable<string> state)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    if (_usersStates.ContainsKey(userId) == true)
                    {
                        _usersStates[userId] = state;
                    }
                    else
                    {
                        _usersStates.Add(userId, state);
                    }
                }
            });
        }
    }
}
