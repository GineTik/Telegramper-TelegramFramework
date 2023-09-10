using Telegramper.Executors.Configuration.Options;
using Telegramper.Executors.Storages.UserState.Saver;
using Telegramper.Core.Context;
using Microsoft.Extensions.Options;

namespace Telegramper.Executors.Storages.UserState
{
    public class UserStateStorage : IUserStateStorage
    {
        private readonly IUserStateSaver _saver;
        private readonly UpdateContext _updateContext;
        private readonly UserStateOptions _options;

        public UserStateStorage(
            IUserStateSaver saver, 
            UpdateContextAccessor accessor, 
            IOptions<UserStateOptions> options)
        {
            _saver = saver;
            _updateContext = accessor.UpdateContext;
            _options = options.Value;
        }

        public async Task AddAsync(string state, long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;

            var userStates = await GetAsync(telegramUserId);
            userStates = userStates.Concat(new[] { state });

            await SetRangeAsync(userStates, telegramUserId);
        }

        public async Task<IEnumerable<string>> GetAsync(long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            ArgumentNullException.ThrowIfNull(telegramUserId);

            var userStates = await _saver.GetAsync(telegramUserId.Value);
            return userStates ?? new[] { _options.DefaultUserState };
        }

        public async Task RemoveAsync(long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            ArgumentNullException.ThrowIfNull(telegramUserId);

            await _saver.RemoveAsync(telegramUserId.Value);
        }

        public async Task SetAsync(string state, long? telegramUserId = null, bool withDefaultState = false)
        {
            await SetRangeAsync(new[] { state }, telegramUserId, withDefaultState);
        }

        public async Task SetRangeAsync(IEnumerable<string> states, long? telegramUserId = null, bool withDefaultState = false)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            ArgumentNullException.ThrowIfNull(telegramUserId);

            if (withDefaultState == true)
            {
                states = states.Concat(new List<string> { _options.DefaultUserState });
            }

            await _saver.AddAsync(telegramUserId.Value, states);
        }
    }
}
