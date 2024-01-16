using System.Reflection;
using Telegramper.Dialog.Attributes;
using Telegramper.Dialog.Models;
using Telegramper.Executors.Common.Models;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Dialog.StorageInitializers
{
    public class SequenceStorageInitializer : IDictionaryStorageInitializer<SequenceDictionary>
    {
        private readonly IEnumerable<ExecutorMethod> _executorMethods;
        private readonly IEnumerable<ExecutorType> _executorTypes;

        public SequenceStorageInitializer(IListStorage<ExecutorMethod> executorMethodStorage, IListStorage<ExecutorType> executorTypes)
        {
            _executorTypes = executorTypes.Items;
            _executorMethods = executorMethodStorage.Items;
        }

        public SequenceDictionary Initialization()
        {
            return findSteps(_executorMethods);
        }

        private SequenceDictionary findSteps(IEnumerable<ExecutorMethod> methods)
        {
            var sequenceDictionary = new SequenceDictionary();
            
            addStepsOfSequences(methods, sequenceDictionary);
            addStartOfSequences(sequenceDictionary);
            addEndOfSequences(sequenceDictionary);

            return sequenceDictionary;
        }

        private static void addStepsOfSequences(IEnumerable<ExecutorMethod> methods, SequenceDictionary sequenceDictionary)
        {
            foreach (var method in methods)
            {
                if (method.TargetAttributes.FirstOrDefault(t => t is TargetSequenceStepAttribute) is not
                    TargetSequenceStepAttribute sequenceStepAttribute) continue;

                var sequence = getSequence(sequenceStepAttribute.SequenceName, sequenceDictionary);
                sequence.Steps.Add(new SequenceStep
                {
                    Method = method,
                    StepAttribute = sequenceStepAttribute
                });
                sequenceDictionary[sequence.Name] = sequence;
            }
        }

        private void addStartOfSequences(SequenceDictionary sequenceDictionary)
        {
            var startOfSequenceMethods = _executorTypes
                .SelectMany(t => t.Type.GetMethods())
                .Where(m => m.GetCustomAttribute<StartOfSequenceAttribute>() != null);

            foreach (var method in startOfSequenceMethods)
            {
                var executorType = method.DeclaringType ??
                                   method.ReflectedType ?? throw new InvalidOperationException("The method haven't class type");
                var sequence =
                    getSequence(method.GetCustomAttribute<StartOfSequenceAttribute>()!.SequenceName ?? executorType.Name,
                        sequenceDictionary);
                sequence.StartOfSequence = method;
            }
        }

        private void addEndOfSequences(SequenceDictionary sequenceDictionary)
        {
            var endOfSequenceMethods = _executorTypes
                .SelectMany(t => t.Type.GetMethods())
                .Where(m => m.GetCustomAttribute<EndOfSequenceAttribute>() != null);

            foreach (var method in endOfSequenceMethods)
            {
                var executorType = method.DeclaringType ??
                                   method.ReflectedType ?? throw new InvalidOperationException("The method haven't class type");
                var sequence =
                    getSequence(method.GetCustomAttribute<EndOfSequenceAttribute>()!.SequenceName ?? executorType.Name,
                        sequenceDictionary);
                sequence.EndOfSequence = method;
            }
        }

        private static Sequence getSequence(string sequenceName, SequenceDictionary sequenceDictionary)
        {
            sequenceDictionary.TryGetValue(sequenceName, out var sequence);
            return sequence ?? new Sequence
            {
                Name = sequenceName
            };
        }
    }
}
