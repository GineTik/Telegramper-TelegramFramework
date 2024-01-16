namespace Telegramper.Executors.QueryHandlers.UserState.Strategy
{
    public class MemoryUserStateSaveStrategy : IUserStateSaveStrategy
    {
        private readonly Dictionary<long, HashSet<string>> _usersStates = new();
        private readonly object _locker = new();

        public async Task SetRangeAsync(long userId, IEnumerable<string> states)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    _usersStates[userId] = new HashSet<string>(states);
                }
            });
        }

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

        public async Task RemoveAllAsync(long userId)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    _usersStates.Remove(userId);
                }
            });
        }

        public async Task RemoveAsync(long userId, string state)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    if (_usersStates.TryGetValue(userId, out var usersState))
                        usersState.Remove(state);
                }
            });
        }

        public async Task AddRangeAsync(long userId, IEnumerable<string> state)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    _usersStates.TryGetValue(userId, out var userState);
                    _usersStates[userId] = new HashSet<string>(userState == null
                        ? state
                        : state.Concat(userState));
                }
            });
        }
    }
}
