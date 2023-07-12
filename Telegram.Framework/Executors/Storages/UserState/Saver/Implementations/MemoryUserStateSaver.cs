namespace Telegram.Framework.Executors.Storages.UserState.Saver.Implementations
{
    public class MemoryUserStateSaver : IUserStateSaver
    {
        private readonly Dictionary<long, IEnumerable<string>> _usersStates;

        public MemoryUserStateSaver()
        {
            _usersStates = new();
        }

        public async Task<IEnumerable<string>?> LoadAsync(long userId)
        {
            return await Task.Run(() =>
            {
                if (_usersStates.ContainsKey(userId) == false)
                    return null;

                return _usersStates[userId];
            });
        }

        public async Task RemoveAsync(long userId)
        {
            await Task.Run(() => _usersStates.Remove(userId));
        }

        public async Task SaveAsync(long userId, IEnumerable<string> state)
        {
            await Task.Run(() =>
            {
                if (_usersStates.ContainsKey(userId) == true)
                {
                    _usersStates[userId] = state!;
                }
                else
                {
                    _usersStates.Add(userId, state!);
                }
            });
        }
    }
}
