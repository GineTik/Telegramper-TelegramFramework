using Telegram.Bot;
using Telegramper.Core.AdvancedBotClient.Extensions;
using Telegramper.Core.Context;
using Telegramper.Dialog.Models;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Storage.Dictionary;

namespace Telegramper.Dialog.Service
{
    public class DialogService : IDialogService
    {
        private readonly IUserStates _userStates;
        private readonly DialogStepsDictionary _steps;
        private readonly UpdateContext _updateContext;

        public DialogService(
            IUserStates userStates,
            IDictionaryStorage<DialogStepsDictionary> stepStorage,
            UpdateContextAccessor updateContextAccessor)
        {
            _userStates = userStates;
            _steps = stepStorage.Items;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        public async Task StartAsync(string dialogName)
        {
            if (_steps.TryGetValue(dialogName, out var steps) == false)
            {
                throw new ArgumentException("The dialog with current name not exists");
            }

            // The steps will definitely be one or more
            var firstStep = steps.First();
            await runStepAsync(firstStep);
        }

        public async Task StartAsync<T>() where T : Executor
        {
            await StartAsync(typeof(T).Name);
        }

        public async Task NextAsync()
        {
            if (await IsLaunchedAsync() == false)
            {
                throw new InvalidOperationException("The dialog is not running");
            }

            var states = await _userStates.GetAsync();
            var dialogParams = states.First(state => state.StartsWith(DialogConstants.Modificator)).Split(":");
            var dialogName = dialogParams[1];
            var currentStepIndex = int.Parse(dialogParams[2]);
            var nextStepIndex = ++currentStepIndex;

            if (_steps.TryGetValue(dialogName, out var steps) == false)
            {
                throw new ArgumentException("The dialog with current name not exists");
            }

            if (steps.Count <= nextStepIndex)
            {
                await EndAsync();
                return;
            }

            var nextStep = steps.ElementAt(nextStepIndex);
            await runStepAsync(nextStep);
        }

        public async Task EndAsync()
        {
            await _userStates.RemoveAsync();
        }

        public async Task<bool> IsLaunchedAsync()
        {
            var states = await _userStates.GetAsync();
            return states.Any(state => state.StartsWith(DialogConstants.Modificator));
        }

        private async Task runStepAsync(DialogStep step)
        {
            var stepAttribute = step.StepAttribute;

            await _userStates.SetRangeAsync(
                stepAttribute.UserStates
                .Append(StaticDialogUserStateFactory.Create(stepAttribute.DialogName))
            );

            if (stepAttribute.Key != null)
            {
                await _userStates.AddAsync(
                    StaticDialogUserStateFactory.CreateByKey(
                        stepAttribute.DialogName,
                        stepAttribute.Key
                    )
                );
            }

            if (stepAttribute.Question != null)
            {
                await _updateContext.Client.SendTextMessageAsync(
                    stepAttribute.Question,
                    parseMode: stepAttribute.ParseMode
                );
            }
        }
    } 
}
