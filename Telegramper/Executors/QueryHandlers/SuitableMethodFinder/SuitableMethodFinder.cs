using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Storage.Dictionary;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder
{
    public class SuitableMethodFinder : ISuitableMethodFinder
    {
        private readonly UpdateContext _updateContext;
        private readonly RouteTree _routes;
        private readonly IUserStates _userStates;

        public SuitableMethodFinder(
            UpdateContextAccessor updateContextAccessor,
            IDictionaryStorage<RouteTree> routes,
            IUserStates userStates)
        {
            _updateContext = updateContextAccessor.UpdateContext;
            _routes = routes.Items;
            _userStates = userStates;
        }

        public async Task<IEnumerable<ExecutorMethod>> FindForCurrentUpdateAsync()
        {
            var userStates = await _userStates.GetAsync(_updateContext.TelegramUserId);

            var methods = _routes.GetTargetMethodInfos(
                _updateContext.Update.Type,
                userStates
            );

            var targetMethods = methods.Where(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(_updateContext.Update))
            );

            return targetMethods.Select(m => m.Method);
        }
    }
}
