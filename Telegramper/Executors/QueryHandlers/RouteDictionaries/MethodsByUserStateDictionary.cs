using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.RouteDictionaries
{
    public class MethodsByUserStateDictionary : Dictionary<string, MethodsByUserState>
    {
        public void AddOrSet(string userStateKey, Route temporaryMethodData)
        {
            if (TryGetValue(userStateKey, out var foundMethodsByUserState))
            {
                foundMethodsByUserState.Add(temporaryMethodData);
            }
            else
            {
                var methodsByUserState = new MethodsByUserState();
                methodsByUserState.Add(temporaryMethodData);
                Add(userStateKey, methodsByUserState);
            }
        }
        
        public IEnumerable<MethodsByUserState> GetSuitableMethodsBy(IEnumerable<string> userStates)
        {
            foreach (var state in userStates)
            {
                if (!TryGetValue(state, out var methods)) continue;
                yield return methods;
            }
        }
    }
}
