using System.Reflection;
using Telegramper.Executors.Common.Models;
using Telegramper.Sequence.Attributes;
using Telegramper.Sequence.Models;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Sequence.StorageInitializers
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
            addStartOrEndOfSequence<StartOfSequenceAttribute>(
                sequenceDictionary, 
                (sequence, method) => sequence.StartOfSequence = method);
        }

        private void addEndOfSequences(SequenceDictionary sequenceDictionary)
        {
            addStartOrEndOfSequence<EndOfSequenceAttribute>(
                sequenceDictionary, 
                (sequence, method) => sequence.EndOfSequence = method);
        }

        private void addStartOrEndOfSequence<TAttribute>(SequenceDictionary sequenceDictionary, Action<Models.Sequence, MethodInfo> setProperty)
            where TAttribute : Attribute 
        {
            var endOfSequenceMethods = _executorTypes
                .SelectMany(t => t.Type.GetMethods())
                .Where(m => m.GetCustomAttribute<TAttribute>() != null);

            foreach (var method in endOfSequenceMethods)
            {
                var executorType = method.DeclaringType ??
                                   method.ReflectedType ?? 
                                   throw new InvalidOperationException("The method haven't class type");
                var sequence = getSequence(executorType.FullName!, sequenceDictionary);
                setProperty(sequence, method);
            }
        }

        private static Models.Sequence getSequence(string sequenceName, SequenceDictionary sequenceDictionary)
        {
            sequenceDictionary.TryGetValue(sequenceName, out var sequence);
            return sequence ?? new Models.Sequence
            {
                Name = sequenceName
            };
        }
    }
}
