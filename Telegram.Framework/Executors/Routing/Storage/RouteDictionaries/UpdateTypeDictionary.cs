using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types.Enums;
using Telegram.Framework.Executors.Routing.Storage.Models;

namespace Telegram.Framework.Executors.Routing.Storage.RouteDictionaries
{
    public class UpdateTypeDictionary : Dictionary<UpdateType, UserStateMethodInfoDictionary>
    {
        private string _defaultUserState;

        public UpdateTypeDictionary(string defaultUserState)
        {
            _defaultUserState = defaultUserState;

            var updateTypes = getExistngUpdateTypes();

            foreach (var updateType in updateTypes)
            {
                Add(updateType, new UserStateMethodInfoDictionary());
            }
        }

        public void AddMethods(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                AddMethod(method);
            }
        }

        public void AddMethod(MethodInfo method)
        {
            var targetAttributes = method.GetCustomAttributes<TargetAttribute>();

            foreach (var targetAttribute in targetAttributes)
            {
                var updateTypes = getUpdateTypesOf(targetAttribute);

                if (updateTypes.Count() == 0)
                {
                    updateTypes = new List<UpdateType>() { UpdateType.Unknown };
                }

                foreach (var updateType in updateTypes)
                {
                    addMethodToUserStates(method, targetAttribute, updateType);
                }
            }
        }

        public IEnumerable<TargetMethodInfo> GetTargetMethodInfos(UpdateType updateType, IEnumerable<string> states)
        {
            var methods = this[updateType].GetTargetMethodInfos(states);

            if (updateType == UpdateType.Unknown)
            {
                return methods;
            }

            var unknownMethods = this[UpdateType.Unknown].GetTargetMethodInfos(states);
            return methods.Concat(unknownMethods);
        }

        private void addMethodToUserStates(MethodInfo method, TargetAttribute targetAttribute, UpdateType updateType)
        {
            var userStateDictionary = this[updateType];

            targetAttribute.UserStates ??= _defaultUserState;
            IEnumerable<string> userStates = targetAttribute.UserStates
                .Split(",")
                .Select(s => s.Trim());

            foreach (var state in userStates)
            {
                userStateDictionary.AddMethod(state, method, targetAttribute);
            }
        }

        private static IEnumerable<UpdateType> getExistngUpdateTypes()
        {
            return Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();
        }

        private static IEnumerable<UpdateType> getUpdateTypesOf(TargetAttribute targetAttribute)
        {
            return targetAttribute
                .GetType()
                .GetCustomAttributes<TargetUpdateTypeAttribute>()
                .Select(attr => attr.UpdateType);
        }
    }
}
