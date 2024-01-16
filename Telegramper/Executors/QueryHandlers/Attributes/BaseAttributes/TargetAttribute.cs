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
        public string[] UserStates
        {
            get => buildUserStates();
            set => _userStatesOfAttribute = value;
        }
        protected string MethodName { get; private set; } = null!;
        protected string TransformedMethodName { get; private set; } = null!;
        protected User Bot { get; private set; } = null!;

        private string _defaultUserState = null!;
        private IEnumerable<string> _userStatesOfMethod = null!;
        private IEnumerable<string> _userStatesOfExecutor = null!;
        private IEnumerable<string> _userStatesOfAttribute = Array.Empty<string>();

        public void Initialization(ExecutorMethod method, IServiceProvider serviceProvider)
        {
            var transformer = serviceProvider.GetRequiredService<INameTransformer>();
            var botAccessor = serviceProvider.GetRequiredService<BotAccessor>();
            var userStateOptions = serviceProvider.GetRequiredService<IOptions<UserStateOptions>>().Value;
            var userStateAttributeOfMethod = method.GetCustomAttribute<UserStateAttribute>();
            var userStateAttributeOfExecutor = method.ExecutorType.GetCustomAttribute<UserStateAttribute>();

            MethodName = method.MethodInfo.Name;
            TransformedMethodName = transformer.Transform(MethodName);
            Bot = botAccessor.Bot;
            UpdateTypes = GetType()
                .GetCustomAttribute<TargetUpdateTypeAttribute>()
                ?.UpdateTypes ?? new[] { UpdateType.Unknown };
            _defaultUserState = userStateOptions.DefaultUserState;
            _userStatesOfMethod = userStateAttributeOfMethod?.UserStates ?? Array.Empty<string>();
            _userStatesOfExecutor = userStateAttributeOfExecutor?.UserStates ?? Array.Empty<string>();

            Initialization(method);
        }

        public abstract bool IsTarget(Update update);
        protected virtual void Initialization(ExecutorMethod method) { }
        
        private string[] buildUserStates()
        {
            var userStates =
                Array.Empty<string>()
                    .Concat(_userStatesOfAttribute)
                    .Concat(_userStatesOfMethod)
                    .Concat(_userStatesOfExecutor)
                    .Select(s => s.Trim())
                    .ToArray();
            return userStates.Any() ? userStates : new[] { _defaultUserState };
        }
    }
}
