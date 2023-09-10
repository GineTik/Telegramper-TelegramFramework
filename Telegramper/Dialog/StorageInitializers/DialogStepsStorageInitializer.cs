using Telegramper.Dialog.Attributes;
using Telegramper.Dialog.Models;
using Telegramper.Executors.Common.Models;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Dialog.StorageInitializers
{
    public class DialogStepsStorageInitializer : IDictionaryStorageInitializer<DialogStepsDictionary>
    {
        private readonly IEnumerable<ExecutorMethod> _executorMethods;

        public DialogStepsStorageInitializer(IListStorage<ExecutorMethod> executorMethodStorage)
        {
            _executorMethods = executorMethodStorage.Items;
        }

        public DialogStepsDictionary Initialization()
        {
            return FindSteps(_executorMethods);
        }

        public DialogStepsDictionary FindSteps(IEnumerable<ExecutorMethod> methods)
        {
            var steps = takeStepsFromMethods(methods);
            var groupedSteps = steps.GroupBy(step => step.StepAttribute.DialogName);

            var builedStepsDictionary = new DialogStepsDictionary();

            foreach (var groupedStep in groupedSteps)
            {
                var sortedSteps = groupedStep.ToList().OrderBy(step => step.StepAttribute.Priority).ToList();
                builedStepsDictionary.Add(groupedStep.Key, sortedSteps);
            }

            return builedStepsDictionary;
        }

        private IEnumerable<DialogStep> takeStepsFromMethods(IEnumerable<ExecutorMethod> methods)
        {
            foreach (var method in methods)
            {
                var stepAttribute = method.TargetAttributes.FirstOrDefault(
                    attr => attr.GetType().Name == nameof(TargetDialogStepAttribute));

                if (stepAttribute == null)
                {
                    continue;
                }

                yield return new DialogStep
                {
                    Method = method,
                    StepAttribute = (TargetDialogStepAttribute)stepAttribute
                };
            }
        }
    }
}
