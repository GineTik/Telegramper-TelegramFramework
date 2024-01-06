using Microsoft.Extensions.Options;
using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder.Strategies;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Storage.Dictionary;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder
{
    public class SuitableMethodFinder : ISuitableMethodFinder
    {
        private readonly UpdateContext _updateContext;
        private readonly RoutesDictionary _routes;
        private readonly IUserStates _userStates;
        private readonly ISuitableMethodFinderStrategy _finderStrategy;

        public SuitableMethodFinder(
            UpdateContextAccessor updateContextAccessor,
            IDictionaryStorage<RoutesDictionary> routes,
            IUserStates userStates, 
            IOptions<HandlerQueueOptions> options,
            ManyFinderStrategy manyFinderStrategy,
            SingleFinderStrategy singleFinderStrategy,
            LimitedFinderStrategy limitedFinderStrategy)
        {
            _updateContext = updateContextAccessor.UpdateContext;
            _routes = routes.Items;
            _userStates = userStates;
            _finderStrategy = options.Value.LimitOfHandlersPerRequest switch
            {
                HandlerQueueOptions.NoneLimit => manyFinderStrategy,
                1 => singleFinderStrategy,
                _ => limitedFinderStrategy,
            };
        }

        public async Task<IEnumerable<Route>> FindForCurrentUpdateAsync()
        {
            var userStates = await _userStates.GetAsync(_updateContext.TelegramUserId);

            var methods = _routes.GetSuitableMethodsBy(
                _updateContext.Update.Type,
                userStates
            ).ToList();

            return _finderStrategy.Find(
                methods.SelectMany(m => m.RoutesInHandlerQueue),
                methods.SelectMany(m => m.RoutesWithIgnoreHandlerAttribute));
        }
    }
}
