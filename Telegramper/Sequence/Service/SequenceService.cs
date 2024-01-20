using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Extensions;
using Telegramper.Executors.QueryHandlers.Factory;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Sequence.Models;
using Telegramper.Storage.Dictionary;

namespace Telegramper.Sequence.Service
{
    public class SequenceService : ISequenceService
    {
        private readonly IUserStates _userStates;
        private readonly SequenceDictionary _sequences;
        private readonly UpdateContext _updateContext;
        private readonly IExecutorFactory _executorFactory;

        public SequenceService(
            IUserStates userStates,
            IDictionaryStorage<SequenceDictionary> stepStorage,
            UpdateContextAccessor updateContextAccessor, IExecutorFactory executorFactory)
        {
            _userStates = userStates;
            _executorFactory = executorFactory;
            _sequences = stepStorage.Items;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        public async Task StartAsync(string sequenceName)
        {
            if (_sequences.TryGetValue(sequenceName, out var sequence) == false)
            {
                throw new ArgumentException($"The sequence with current name ({sequenceName}) not exists");
            }

            if (sequence.StartOfSequence != null)
            {
                await sequence.StartOfSequence!.InvokeMethodAsync(_executorFactory, Array.Empty<object>());
            }
            
            await _userStates.SetRangeAsync(new[]
            {
                StaticSequenceUserStateFactory.Create(sequence.Name),
                StaticSequenceUserStateFactory.CreateByIndex(sequence.Name, 0)
            });
        }

        public async Task ShiftAsync(int offset = 1)
        {
            if (await IsLaunchedAsync() == false)
            {
                throw new InvalidOperationException("The sequence is not running");
            }

            var states = (await _userStates.GetAsync()).ToList();
            var sequenceName = getSequenceNameFromUserStates(states);
            var currentStepIndex = int.Parse(getStepIndexFromUserState(states));
            var nextStepIndex = currentStepIndex + offset;

            if (_sequences.TryGetValue(sequenceName, out var sequence) == false)
            {
                throw new ArgumentException($"The sequence with current name({sequenceName}) not exists");
            }
            
            if (nextStepIndex < 0 || nextStepIndex >= sequence.Steps.Count)
            {
                await endAsync(sequence, executeEndCallback: true, currentStepIndex);
                return;
            }
            
            await _userStates.RemoveAsync(StaticSequenceUserStateFactory.CreateByIndex(sequenceName, currentStepIndex));
            await _userStates.AddAsync(StaticSequenceUserStateFactory.CreateByIndex(sequenceName, nextStepIndex));
        }

        public async Task EndAsync(bool executeEndCallback = true)
        {
            var states = (await _userStates.GetAsync()).ToList();
            var sequenceName = getSequenceNameFromUserStates(states);
            var currentStepIndex = int.Parse(getStepIndexFromUserState(states));
            
            if (_sequences.TryGetValue(sequenceName, out var sequence))
            {  
                await endAsync(sequence, executeEndCallback, currentStepIndex);
            }
        }

        public async Task<bool> IsLaunchedAsync()
        {
            return (await _userStates.GetAsync()).Any(s => s.StartsWith(SequenceConstants.ModificatorForName));
        }
        
        private async Task endAsync(Models.Sequence sequence, bool executeEndCallback, int currentStepIndex)
        {
            if (executeEndCallback && sequence.EndOfSequence != null)
            {
                await sequence.EndOfSequence?.InvokeMethodAsync(_executorFactory, Array.Empty<object>())!;
            }

            await _userStates.RemoveAsync(StaticSequenceUserStateFactory.CreateByIndex(sequence.Name, currentStepIndex));
            await _userStates.RemoveAsync(StaticSequenceUserStateFactory.Create(sequence.Name));
        }
        
        private static string getSequenceNameFromUserStates(IEnumerable<string> states)
        {
            return states
                .First(state => state.StartsWith(SequenceConstants.ModificatorForName + SequenceConstants.Separator))
                .Split(SequenceConstants.Separator)
                .Last();
        }

        private static string getStepIndexFromUserState(IEnumerable<string> states)
        {
            return states
                .First(state => state.StartsWith(SequenceConstants.ModificatorForStepIndex))
                .Split(SequenceConstants.Separator)
                .Last();
        }
    } 
}
