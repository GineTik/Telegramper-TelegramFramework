using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.RouteDictionaries
{
    public class RoutesDictionary : Dictionary<UpdateType, MethodsByUserStateDictionary>
    {
        public RoutesDictionary()
        {
            foreach (var updateType in Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>())
            {
                Add(updateType, new MethodsByUserStateDictionary());
            }
        }
        
        public IEnumerable<MethodsByUserState> GetSuitableMethodsBy(UpdateType updateType, IEnumerable<string> states)
        {
            var methods = this[updateType].GetSuitableMethodsBy(states);
        
            return updateType == UpdateType.Unknown 
                ? methods 
                : methods.Concat(this[UpdateType.Unknown].GetSuitableMethodsBy(states));
        }
    }
}
