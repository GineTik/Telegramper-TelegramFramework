using Microsoft.Extensions.Options;
using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.RouteDictionaries;
using Telegramper.Executors.QueryHandlers.UserState;
using Telegramper.Storage.Dictionary;

namespace Telegramper.Executors.QueryHandlers.SuitableMethodFinder
{
    public class SuitableMethodFinder : ISuitableMethodFinder
    {
        private readonly UpdateContext _updateContext;
        private readonly RoutesDictionary _routes;
        private readonly IUserStates _userStates;
        private readonly HandlerQueueOptions _options;
        private readonly Func<IEnumerable<MethodsByUserState>, IEnumerable<ExecutorMethod>> _findSuitableMethods;

        public SuitableMethodFinder(
            UpdateContextAccessor updateContextAccessor,
            IDictionaryStorage<RoutesDictionary> routes,
            IUserStates userStates, 
            IOptions<HandlerQueueOptions> options)
        {
            _updateContext = updateContextAccessor.UpdateContext;
            _routes = routes.Items;
            _userStates = userStates;
            _options = options.Value;
            _findSuitableMethods = _options.LimitOfHandlersPerRequest == HandlerQueueOptions.NoneLimit ? findWithoutLimit : findWithLimit;
        }

        public async Task<IEnumerable<ExecutorMethod>> FindForCurrentUpdateAsync()
        {
            var userStates = await _userStates.GetAsync(_updateContext.TelegramUserId);

            var methods = _routes.GetTargetMethodInfos(
                _updateContext.Update.Type,
                userStates
            );
            
            return _findSuitableMethods(methods);
        }

        private IEnumerable<ExecutorMethod> findWithoutLimit(IEnumerable<MethodsByUserState> methodsByUserStates)
        {
            var result = new List<ExecutorMethod>();

            var methods =
                methodsByUserStates.SelectMany(m =>
                    m.MethodsInHandlerQueue.Concat(m.MethodsWithIgnoreHandlerAttribute));
            result.AddRange(findSuitableMethodsFrom(methods));

            return result;
        }

        private IEnumerable<ExecutorMethod> findWithLimit(IEnumerable<MethodsByUserState> methodsByUserStates)
        {
            var i = 0;
            var targetMethods = new List<ExecutorMethod>();

            var methodsByUserStatesList = methodsByUserStates.ToList();
            foreach (var method in methodsByUserStatesList.SelectMany(m => m.MethodsInHandlerQueue))
            {
                var targetAttributes = method.TargetAttributes;
                if (!targetAttributes.Any(attr => attr.IsTarget(_updateContext.Update))) continue;

                targetMethods.Add(method.Method);
                i++;

                if (i == _options.LimitOfHandlersPerRequest)
                    break;
            }

            var methodsWithIgnoresAttribute = findSuitableMethodsFrom(
                methodsByUserStatesList.SelectMany(m => m.MethodsWithIgnoreHandlerAttribute));
            return targetMethods.Concat(methodsWithIgnoresAttribute);
        }
        
        private IEnumerable<ExecutorMethod> findSuitableMethodsFrom(IEnumerable<RouteMethod> methods)
        {
            return methods.Where(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(_updateContext.Update))
            ).Select(m => m.Method);
        }
    }
}
