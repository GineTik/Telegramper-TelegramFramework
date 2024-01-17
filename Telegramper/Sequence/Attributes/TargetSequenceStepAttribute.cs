using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Telegramper.Sequence.Attributes
{
    [TargetUpdateType(UpdateType.Unknown)]
    public class TargetSequenceStepAttribute : TargetAttribute
    {
        public string SequenceName { get; private set; } = default!;
        
        public string? Name { get; set; }
        public int Priority { get; set; }
        public int Index { get; private set; }

        public override bool IsTarget(Update update)
        {
            return true;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            SequenceName = 
                method.ExecutorType.GetCustomAttribute<SequenceNameAttribute>()?.SequenceName
                ?? method.ExecutorType.Name;

            Index = method.ExecutorType
                .GetMethods()
                .Where(methodInfo => methodInfo.GetCustomAttribute<TargetSequenceStepAttribute>() != null)
                .OrderByDescending(methodInfo => methodInfo.GetCustomAttribute<TargetSequenceStepAttribute>()!.Priority)
                .ToList()
                .IndexOf(method.MethodInfo);

            // TODO: rewrite the saving of sequence steps in states
            UserStates = new[]
            {
                StaticDialogUserStateFactory.CreateByIndex(Index),
                StaticDialogUserStateFactory.CreateByName(Name ?? MethodName),
                // should be the last, because if it is the first in the states, then the first step of the sequence will be executed constantly  
                StaticDialogUserStateFactory.Create(SequenceName),
            };
        }
    }
}
