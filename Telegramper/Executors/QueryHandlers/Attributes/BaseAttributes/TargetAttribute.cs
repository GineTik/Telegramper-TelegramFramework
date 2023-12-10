using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Core;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.QueryHandlers.Attributes.Supports;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public IEnumerable<UpdateType> UpdateTypes { get; private set; } = null!;
        public virtual string? UserStates { get; set; }

        public IEnumerable<string> UserStatesAsEnumerable
        {
            get
            {
                UserStates ??= _defaultUserState;
                return UserStates
                    .Split(",")
                    .Select(s => s.Trim())
                    .Concat(_userStatesOfMethod);
            }
        }

        protected string MethodName { get; private set; } = null!;
        protected string TransformedMethodName { get; private set; } = null!;
        protected User Bot { get; private set; } = null!;

        private string _defaultUserState = null!;
        private IEnumerable<string> _userStatesOfMethod = null!;

        public void Initialization(ExecutorMethod method, IServiceProvider serviceProvider)
        {
            var transformer = serviceProvider.GetRequiredService<INameTransformer>();
            var currentBot = serviceProvider.GetRequiredService<CurrentBot>();
            var userStateOptions = serviceProvider.GetRequiredService<IOptions<UserStateOptions>>().Value;
            var userStateAttribute = method.GetCustomAttribute<UserStateAttribute>();

            MethodName = method.MethodInfo.Name;
            TransformedMethodName = transformer.Transform(MethodName);
            Bot = currentBot.Data;
            UpdateTypes = GetType()
                .GetCustomAttribute<TargetUpdateTypeAttribute>()
                ?.UpdateTypes ?? new[] { UpdateType.Unknown };
            _defaultUserState = userStateOptions.DefaultUserState;
            _userStatesOfMethod = userStateAttribute?.UserStates ?? Array.Empty<string>();

            Initialization(method);
        }

        public abstract bool IsTarget(Update update);
        protected virtual void Initialization(ExecutorMethod method) { }
    }
}
