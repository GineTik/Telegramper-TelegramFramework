using Telegramper.Core.Context;
using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.UserState.Strategy;

namespace Telegramper.Executors.QueryHandlers.UserState
{
    public class UserStates : IUserStates
    {
        private readonly IUserStateSaveStrategy _saveStrategy; 
        private readonly UpdateContext _updateContext;
        private readonly UserStateOptions _options;

        public UserStates(
            IUserStateSaveStrategy saveStrategy,
            UpdateContextAccessor accessor,
            IOptions<UserStateOptions> options)
        {
            _saveStrategy = saveStrategy;
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

            var userStates = await _saveStrategy.GetAsync(telegramUserId.Value);
            return userStates ?? new[] { _options.DefaultUserState };
        }

        public async Task RemoveAsync(long? telegramUserId = null)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            ArgumentNullException.ThrowIfNull(telegramUserId);

            await _saveStrategy.RemoveAsync(telegramUserId.Value);
        }

        public async Task<bool> Contains(string state, long? telegramUserId = null)
        {
            return (await GetAsync(telegramUserId)).Contains(state);
        }

        public async Task SetAsync(string state, long? telegramUserId = null, bool withDefaultState = false)
        {
            await SetRangeAsync(new[] { state }, telegramUserId, withDefaultState);
        }

        public async Task SetRangeAsync(IEnumerable<string> states, long? telegramUserId = null, bool withDefaultState = false)
        {
            telegramUserId ??= _updateContext.TelegramUserId;
            ArgumentNullException.ThrowIfNull(telegramUserId);

            if (withDefaultState)
            {
                states = states.Append(_options.DefaultUserState);
            }

            await _saveStrategy.AddOrUpdateAsync(telegramUserId.Value, states);
        }
    }
}
