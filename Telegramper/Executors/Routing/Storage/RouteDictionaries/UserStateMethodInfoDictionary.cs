using Telegramper.Attributes.BaseAttributes;
using System.Reflection;
using Telegramper.Executors.Routing.Storage.Models;

namespace Telegramper.Executors.Routing.Storage.RouteDictionaries
{
    public class UserStateMethodInfoDictionary : Dictionary<string, ICollection<TargetMethodInfo>>
    {
        public void AddMethod(string state, MethodInfo method, TargetAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(state);
            ArgumentNullException.ThrowIfNull(method);
            ArgumentNullException.ThrowIfNull(attribute);

            if (ContainsKey(state) == false)
            {
                Add(state, new List<TargetMethodInfo>());
            }

            var targetMethods = this[state];
            var targetMethod = targetMethods.FirstOrDefault(info => info.MethodInfo == method);

            if (targetMethod == null)
            {
                targetMethods.Add(new TargetMethodInfo
                {
                    MethodInfo = method,
                    TargetAttributes = new List<TargetAttribute> { attribute }
                });
            }
            else
            {
                targetMethod.TargetAttributes.Add(attribute);
            }
        }

        public IEnumerable<TargetMethodInfo> GetTargetMethodInfos(IEnumerable<string> userStates)
        {
            foreach (var state in userStates)
            {
                TryGetValue(state, out var methods);

                foreach (var method in methods ?? new List<TargetMethodInfo>())
                {
                    yield return method;
                }
            }
        }
    }
}
