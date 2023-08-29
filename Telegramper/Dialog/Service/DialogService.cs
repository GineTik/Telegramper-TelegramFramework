using Telegram.Bot.Requests;
using Telegramper.Dialog.Storages;
using Telegramper.Executors.Routing.UserState;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Dialog.Instance
{
    public class DialogService : IDialogService
    {
        private readonly IUserStates _userStates;
        private readonly IDialogStepStorage _stepStorage;
        private readonly UpdateContext _updateContext;

        public DialogService(
            IUserStates userStates,
            IDialogStepStorage stepStorage,
            UpdateContextAccessor updateContextAccessor)
        {
            _userStates = userStates;
            _stepStorage = stepStorage;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        public async Task StartAsync(string dialogName)
        {
            if (_stepStorage.Steps.TryGetValue(dialogName, out var steps) == false)
            {
                throw new ArgumentException("The dialog with current name not exists");
            }

            // the steps will definitely be one or more
            var firstStep = steps.First();
            var userstate = firstStep.StepAttribute.UserStates!;
            var question = firstStep.StepAttribute.Question;

            await _userStates.SetAsync(userstate);
            await _updateContext.Client.SendTextMessageAsync(question);
        }

        public async Task NextAsync()
        {
            if (await IsLaunchedAsync() == false)
            {
                throw new InvalidOperationException("The dialog is not running");
            }

            var states = await _userStates.GetAsync();
            var dialogParams = states.First(state => state.StartsWith(DialogConstants.ModificatorKey)).Split(":");
            var dialogName = dialogParams[1];
            int currentStepIndex = int.Parse(dialogParams[2]);
            int nextStepIndex = ++currentStepIndex;

            if (_stepStorage.Steps.TryGetValue(dialogName, out var steps) == false)
            {
                throw new ArgumentException("The dialog with current name not exists");
            }

            if (steps.Count <= nextStepIndex)
            {
                await EndAsync();
                return;
            }

            var nextStep = steps.ElementAt(nextStepIndex);
            var userstate = nextStep.StepAttribute.UserStates!;
            var question = nextStep.StepAttribute.Question;

            await _userStates.SetAsync(userstate);
            await _updateContext.Client.SendTextMessageAsync(question);
        }

        public async Task EndAsync()
        {
            // TODO: Remove namely dialog user state
            await _userStates.RemoveAsync();
        }

        public async Task<bool> IsLaunchedAsync()
        {
            var states = await _userStates.GetAsync();
            return states.Any(state => state.StartsWith(DialogConstants.ModificatorKey));
        }
    } 
}
