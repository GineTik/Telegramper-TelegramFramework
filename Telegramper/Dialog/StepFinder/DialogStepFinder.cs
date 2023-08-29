using System.Reflection;
using Telegramper.Dialog.Attributes;
using Telegramper.Executors;

namespace Telegramper.Dialog.StepFinder
{
    public sealed class DialogStepFinder : IDialogStepFinder
    {
        public IDictionary<string, ICollection<DialogStep>> FindOf(IEnumerable<ExecutorMethod> methods)
        {
            var steps = takeStepsFromMethods(methods);
            var groupedSteps = steps.GroupBy(step => step.StepAttribute.DialogName);

            var builedStepsDictionary = new Dictionary<string, ICollection<DialogStep>>();
            
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
