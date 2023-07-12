using Telegram.Framework.Executors.Configuration.Options;
using Telegram.Framework.Executors.Storages.TargetMethod.RouteDictionaries;
using Telegram.Framework.Executors.Storages.TargetMethod.StaticHelpers;
using Telegram.Framework.Executors.Storages.UserState;
using Telegram.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Telegram.Framework.Executors.Storages.TargetMethod
{
    public sealed class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        private readonly UpdateTypeDictionary _routes;
        private readonly IUserStateStorage _stateStorage;
        private readonly UserStateOptions _userStateOptions;

        public TargetMethodStorage(IOptions<TargetMethodOptinons> targetMethodOptions, IUserStateStorage stateStorage,
            IOptions<UserStateOptions> userStateOptions)
        {
            Methods = ExecutorMethodsHelper.TakeExecutorMethodsFrom(targetMethodOptions.Value.ExecutorsTypes);
            _stateStorage = stateStorage;
            _userStateOptions = userStateOptions.Value;
            _routes = new UpdateTypeDictionary(_userStateOptions.DefaultUserState);
            _routes.AddMethods(Methods);
        }

        public async Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext actualUpdateContext)
        {
            var userStates = await _stateStorage.GetAsync(actualUpdateContext.TelegramUserId);
            
            var methods = _routes.GetTargetMethodInfos(
                actualUpdateContext.Update.Type,
                userStates
            );

            var targetMethod = methods.FirstOrDefault(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(actualUpdateContext.Update))
            );

            return targetMethod?.MethodInfo;
        }
    }
}
