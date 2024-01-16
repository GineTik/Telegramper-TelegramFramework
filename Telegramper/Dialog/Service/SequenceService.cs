using Telegramper.Core.Context;
using Telegramper.Dialog.Models;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Extensions;
using Telegramper.Executors.QueryHandlers.Factory;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Storage.Dictionary;

namespace Telegramper.Dialog.Service
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

        public async Task StartAsync(string dialogName)
        {
            if (_sequences.TryGetValue(dialogName, out var sequence) == false)
            {
                throw new ArgumentException("The dialog with current name not exists");
            }

            if (sequence.StartOfSequence != null)
            {
                await sequence.StartOfSequence!.InvokeMethodAsync(_executorFactory, Array.Empty<object>());
            }
            
            // The steps will definitely be one or more
            var firstStep = sequence.Steps.First();
            await setUserStateForStepAsync(firstStep);
        }

        public async Task StartAsync<T>() where T : Executor
        {
            await StartAsync(typeof(T));
        }

        public async Task StartAsync(Type sequenceType)
        {
            await StartAsync(sequenceType.Name);
        }

        public async Task NextAsync()
        {
            if (await IsLaunchedAsync() == false)
            {
                throw new InvalidOperationException("The sequence is not running");
            }

            var states = (await _userStates.GetAsync()).ToList();
            var sequenceName = getSequenceNameFromUserStates(states);
            var currentStepIndex = int.Parse(getStepIndexFromUserState(states));
            var nextStepIndex = ++currentStepIndex;

            if (_sequences.TryGetValue(sequenceName, out var sequence) == false)
            {
                throw new ArgumentException($"The dialog with current name({sequenceName}) not exists");
            }

            if (sequence.Steps.Count <= nextStepIndex)
            {
                await endAsync(sequence);
                return;
            }

            await cleanDialogStates();
            var nextStep = sequence.Steps.ElementAt(nextStepIndex);
            await setUserStateForStepAsync(nextStep);
        }

        public async Task EndAsync()
        {
            var states = await _userStates.GetAsync();
            var sequenceName = getSequenceNameFromUserStates(states);
            if (_sequences.TryGetValue(sequenceName, out var sequence))
            {  
                await endAsync(sequence);
            }
        }

        public async Task<bool> IsLaunchedAsync()
        {
            return (await _userStates.GetAsync()).Any(s => s.StartsWith(SequenceConstants.ModificatorForName));
        }
        
        private async Task endAsync(Sequence sequence)
        {
            if (sequence.EndOfSequence != null)
            {
                await sequence.EndOfSequence?.InvokeMethodAsync(_executorFactory, Array.Empty<object>())!;
            }
            
            await cleanDialogStates();
        }

        private async Task setUserStateForStepAsync(SequenceStep step)
        {
            await _userStates.AddRangeAsync(step.StepAttribute.UserStates);
        }
        
        private async Task cleanDialogStates()
        {
            foreach (var state in await _userStates.GetAsync())
                if (state.StartsWith(SequenceConstants.ModificatorForName))
                    await _userStates.RemoveAsync(state);
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
