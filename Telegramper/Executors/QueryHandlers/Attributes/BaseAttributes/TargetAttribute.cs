using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegramper.Core;
using Telegramper.Core.Context;
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
        protected User Bot { get; private set; } = default!;

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
            var currentBot = serviceProvider.GetRequiredService<CurrentBot>();

            MethodName = method.MethodInfo.Name;
            TransformedMethodName = transformer.Transform(MethodName);
            Bot = currentBot.Data;

            Initialization(method);
        }

        public abstract bool IsTarget(Update update);
        protected virtual void Initialization(ExecutorMethod method) { }
    }
}
