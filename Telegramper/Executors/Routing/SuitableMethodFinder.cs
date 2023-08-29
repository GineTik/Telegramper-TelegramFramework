using Telegramper.Executors.Routing.RoutesStorage;
using Telegramper.Executors.Routing.UserState;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Executors.Routing
{
    public class SuitableMethodFinder : ISuitableMethodFinder
    {
        private readonly UpdateContext _updateContext;
        private readonly IRoutesStorage _routesStorage;
        private readonly IUserStates _userStates;

        public SuitableMethodFinder(
            UpdateContextAccessor updateContextAccessor,
            IRoutesStorage routesStorage,
            IUserStates userStates)
        {
            _updateContext = updateContextAccessor.UpdateContext;
            _routesStorage = routesStorage;
            _userStates = userStates;
        }

        public async Task<IEnumerable<ExecutorMethod>> FindSuitableMethodsForCurrentUpdateAsync()
        {
            var userStates = await _userStates.GetAsync(_updateContext.TelegramUserId);

            var methods = _routesStorage.Routes.GetTargetMethodInfos(
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
