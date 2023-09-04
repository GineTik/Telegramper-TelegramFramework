using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.RouteDictionaries
{
    public class UserStateMethodInfoDictionary : Dictionary<string, ICollection<RouteTreeMethod>>
    {
        public void AddMethod(string state, ExecutorMethod method, TargetAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(state);
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(attribute);

            if (ContainsKey(state) == false)
            {
                Add(state, new List<RouteTreeMethod>());
            }

            var targetMethods = this[state];
            var targetMethod = targetMethods.FirstOrDefault(info => info.Method == method);

            if (targetMethod == null)
            {
                targetMethods.Add(new RouteTreeMethod
                {
                    Method = method,
                    TargetAttributes = new List<TargetAttribute> { attribute }
                });
            }
            else
            {
                targetMethod.TargetAttributes.Add(attribute);
            }
        }

        public IEnumerable<RouteTreeMethod> GetTargetMethodInfos(IEnumerable<string> userStates)
        {
            foreach (var state in userStates)
            {
                TryGetValue(state, out var methods);

                foreach (var method in methods ?? new List<RouteTreeMethod>())
                {
                    yield return method;
                }
            }
        }
    }
}
