using System.Reflection;
using Telegram.Bot.Types;
using Telegramper.Executors.NameTransformer;

namespace Telegramper.Executors.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public string? UserStates { get; set; }
        protected string MethodName { get; private set; } = default!;
        protected string TransformedMethodName { get; private set; } = default!;

        public IEnumerable<string> GetUserStatesAsEnumerable(string defaultUserStates)
        {
            UserStates = defaultUserStates;
            return UserStates
                    .Split(",")
                    .Select(s => s.Trim());
        }

        public void InitializationMethodName(MethodInfo methodInfo, INameTransformer transformer)
        {
            MethodName = methodInfo.Name;
            TransformedMethodName = transformer.Transform(methodInfo.Name);
        }

        public abstract bool IsTarget(Update update);
    }
}
