using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Types;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Initialization.NameTransformer;

namespace Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public virtual string? UserStates { get; set; }
        protected string MethodName { get; private set; } = default!;
        protected string TransformedMethodName { get; private set; } = default!;

        public IEnumerable<string> GetUserStatesAsEnumerable(string defaultUserStates)
        {
            UserStates ??= defaultUserStates;
            return UserStates
                    .Split(",")
                    .Select(s => s.Trim());
        }

        public void Initialization(ExecutorMethod method, IServiceProvider serviceProvider)
        {
            var transformer = serviceProvider.GetRequiredService<INameTransformer>();

            MethodName = method.MethodInfo.Name;
            TransformedMethodName = transformer.Transform(MethodName);

            BeforeInitialization(method, serviceProvider);
        }

        public abstract bool IsTarget(Update update);
        protected virtual void BeforeInitialization(ExecutorMethod method, IServiceProvider serviceProvider) { }
    }
}
