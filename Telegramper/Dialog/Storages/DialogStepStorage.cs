using Telegramper.Dialog.StepFinder;
using Telegramper.Executors.Routing.RoutesStorage;

namespace Telegramper.Dialog.Storages
{
    public class DialogStepStorage : IDialogStepStorage
    {
        public IDictionary<string, ICollection<DialogStep>> Steps { get; } = default!;

        public DialogStepStorage(
            IDialogStepFinder dialogStepFinder,
            IRoutesStorage routesStorage)
        {
            Steps = dialogStepFinder.FindOf(routesStorage.Methods);
        } 
    }
}
