namespace Telegramper.Executors.Routing.UserState.Saver.Implementations
{
    public class MemoryUserStateSaver : IUserStateSaver
    {
        private readonly Dictionary<long, IEnumerable<string>> _usersStates;
        private readonly object _locker = new object();

        public MemoryUserStateSaver()
        {
            _usersStates = new();
        }

        public async Task<IEnumerable<string>?> GetAsync(long userId)
        {
            return await Task.Run(() =>
            {
                lock (_locker)
                {
                    if (_usersStates.ContainsKey(userId) == false)
                        return null;

                    return _usersStates[userId];
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

        public async Task AddAsync(long userId, IEnumerable<string> state)
        {
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    if (_usersStates.ContainsKey(userId) == true)
                    {
                        _usersStates[userId] = state!;
                    }
                    else
                    {
                        _usersStates.Add(userId, state!);
                    }
                }
            });
        }
    }
}
