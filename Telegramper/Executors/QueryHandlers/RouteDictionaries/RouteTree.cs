using System.Reflection;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Telegramper.Executors.QueryHandlers.RouteDictionaries
{
    public class RouteTree : Dictionary<UpdateType, UserStateMethodInfoDictionary>
    {
        private readonly string _defaultUserState;

        public RouteTree(string defaultUserState, IEnumerable<ExecutorMethod> executorsMethods)
        {
            _defaultUserState = defaultUserState;

            var updateTypes = getExistngUpdateTypes();

            foreach (var updateType in updateTypes)
            {
                Add(updateType, new UserStateMethodInfoDictionary());
            }

            AddMethods(executorsMethods);
        }

        public void AddMethods(IEnumerable<ExecutorMethod> executorsMethods)
        {
            foreach (var method in executorsMethods)
            {
                AddMethod(method);
            }
        }

        public void AddMethod(ExecutorMethod method)
        {
            var targetAttributes = method.TargetAttributes;

            foreach (var targetAttribute in targetAttributes)
            {
                var updateTypes = takeUpdateTypesOfTargetAttribute(targetAttribute);

                if (updateTypes.Any() == false)
                {
                    updateTypes = new List<UpdateType>() { UpdateType.Unknown };
                }

                foreach (var updateType in updateTypes)
                {
                    addMethodToUserStates(method, targetAttribute, updateType);
                }
            }
        }

        public IEnumerable<RouteTreeMethod> GetTargetMethodInfos(UpdateType updateType, IEnumerable<string> states)
        {
            var methods = this[updateType].GetTargetMethodInfos(states);

            if (updateType == UpdateType.Unknown)
            {
                return methods;
            }

            var unknownMethods = this[UpdateType.Unknown].GetTargetMethodInfos(states);
            return methods.Concat(unknownMethods);
        }

        private void addMethodToUserStates(ExecutorMethod method, TargetAttribute targetAttribute, UpdateType updateType)
        {
            var userStateDictionary = this[updateType];
            var userStates = targetAttribute.GetUserStatesAsEnumerable(_defaultUserState);

            foreach (var state in userStates)
            {
                userStateDictionary.AddMethod(state, method, targetAttribute);
            }
        }

        private static IEnumerable<UpdateType> getExistngUpdateTypes()
        {
            return Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();
        }

        private static IEnumerable<UpdateType> takeUpdateTypesOfTargetAttribute(TargetAttribute targetAttribute)
        {
            return targetAttribute
                .GetType()
                .GetCustomAttributes<TargetUpdateTypeAttribute>()
                .SelectMany(attr => attr.UpdateTypes);
        }
    }
}
