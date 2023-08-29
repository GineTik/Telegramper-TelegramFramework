using Telegramper.Executors.Routing.Attributes.BaseAttributes;
using Telegramper.Executors.Routing.RoutesStorage.Models;

namespace Telegramper.Executors.Routing.RoutesStorage.RouteDictionaries
{
    public class UserStateMethodInfoDictionary : Dictionary<string, ICollection<TargetExecutorMethod>>
    {
        public void AddMethod(string state, ExecutorMethod method, TargetAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(state);
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(attribute);

            if (ContainsKey(state) == false)
            {
                Add(state, new List<TargetExecutorMethod>());
            }

            var targetMethods = this[state];
            var targetMethod = targetMethods.FirstOrDefault(info => info.Method == method);

            if (targetMethod == null)
            {
                targetMethods.Add(new TargetExecutorMethod
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

        public IEnumerable<TargetExecutorMethod> GetTargetMethodInfos(IEnumerable<string> userStates)
        {
            foreach (var state in userStates)
            {
                TryGetValue(state, out var methods);

                foreach (var method in methods ?? new List<TargetExecutorMethod>())
                {
                    yield return method;
                }
            }
        }
    }
}
