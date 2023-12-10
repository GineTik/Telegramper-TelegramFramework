using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
{
    public enum TextMatchingMode
    {
        StartWith,
        EndWith,
        Contains,
        Equal
    }
    
    [TargetUpdateType(UpdateType.Message)]
    public class TargetTextAttribute : TargetAttribute
    {
        private readonly Predicate<Update> _predicate;

        public TargetTextAttribute(string text, TextMatchingMode textMatchingMode = TextMatchingMode.Equal)
        {
            _predicate = textMatchingMode switch
            {
                TextMatchingMode.Equal => (update) => update.Message!.Text == text,
                TextMatchingMode.StartWith => (update) =>update.Message!.Text?.StartsWith(text) ?? false,
                TextMatchingMode.EndWith => (update) =>update.Message!.Text?.EndsWith(text) ?? false,
                TextMatchingMode.Contains => (update) =>update.Message!.Text?.Contains(text) ?? false,
                _ => throw new NotSupportedException()
            };
        }

        public override bool IsTarget(Update update)
        {
            return _predicate(update);
        }
    }
}
