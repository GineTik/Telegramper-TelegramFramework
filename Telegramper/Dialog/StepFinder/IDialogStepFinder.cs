using Telegramper.Executors;

namespace Telegramper.Dialog.StepFinder
{
    public interface IDialogStepFinder
    {
        IDictionary<string, ICollection<DialogStep>> FindOf(IEnumerable<ExecutorMethod> methods);
    }
}
