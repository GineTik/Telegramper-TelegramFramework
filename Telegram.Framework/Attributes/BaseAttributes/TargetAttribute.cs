using Telegram.Bot.Types;

namespace Telegram.Framework.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public string? UserStates { get; set; }

        public IEnumerable<string> GetUserStatesAsEnumerable(string defaultUserStates)
        {
            UserStates = defaultUserStates;
            return UserStates
                    .Split(",")
                    .Select(s => s.Trim());
        }

        public abstract bool IsTarget(Update update);
    }
}
