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

        public override bool IsTarget(Update update)
        {
            return true;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            SequenceName = method.ExecutorType.FullName!;

            var index = method.ExecutorType
                .GetMethods()
                .Where(methodInfo => methodInfo.GetCustomAttribute<TargetSequenceStepAttribute>() != null)
                .ToList()
                .IndexOf(method.MethodInfo);

            UserStates = new[]
            {
                StaticSequenceUserStateFactory.CreateByIndex(SequenceName, index),
            };
        }
    }
}
