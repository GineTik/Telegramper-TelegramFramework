using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Attributes.BaseAttributes;
using Telegramper.Executors.Attributes.TargetExecutorAttributes;
using Telegramper.Executors.NameTransformer;
using Telegramper.Executors.Routing.Storage.Models;

namespace Telegramper.Executors.Routing.Storage.RouteDictionaries
{
    public class UpdateTypeDictionary : Dictionary<UpdateType, UserStateMethodInfoDictionary>
    {
        private readonly string _defaultUserState;
        private readonly IServiceProvider _serviceProvider;

        public UpdateTypeDictionary(IServiceProvider serviceProvider, string defaultUserState)
        {
            _defaultUserState = defaultUserState;
            _serviceProvider = serviceProvider;

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
            initializeTargetAttributes(method, targetAttributes);

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

        private void initializeTargetAttributes(MethodInfo method, IEnumerable<TargetAttribute> targetAttributes)
        {
            foreach (var attribute in targetAttributes)
            {
                var commandNameTransformer = _serviceProvider.GetRequiredService<INameTransformer>();
                attribute.InitializationMethodName(method, commandNameTransformer);
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
                .GetCustomAttributes<TargetUpdateTypesAttribute>()
                .SelectMany(attr => attr.UpdateTypes);
        }
    }
}
