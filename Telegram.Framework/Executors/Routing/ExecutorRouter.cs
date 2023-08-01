using Microsoft.Extensions.Options;
using System.Reflection;
using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Framework.Executors.Configuration.Options;
using Telegram.Framework.Executors.Helpers.Extensions.MethodInfos;
using Telegram.Framework.Executors.Helpers.Factories.Executors;
using Telegram.Framework.Executors.Routing.Models;
using Telegram.Framework.Executors.Routing.ParametersParser;
using Telegram.Framework.Executors.Routing.ParametersParser.Extensions;
using Telegram.Framework.Executors.Routing.ParametersParser.Results;
using Telegram.Framework.Executors.Routing.Storage;
using Telegram.Framework.Executors.Storages.UserState;
using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.Executors.Routing
{
    public class ExecutorRouter : IExecutorRouter
    {
        private readonly IRoutesStorage _methodStorage;
        private readonly IUserStateStorage _stateStorage;
        private readonly UpdateContext _updateContext;
        private readonly ParameterParserOptions _parameterParserOptions;
        private readonly IParametersParser _parameterParser;
        private readonly IServiceProvider _serviceProvider;
        private readonly IExecutorFactory _executorFactory;

        public ExecutorRouter(
            IRoutesStorage methodStorage, 
            UpdateContextAccessor updateContextAccessor, 
            IUserStateStorage stateStorage,
            IOptions<ParameterParserOptions> parameterParserOptions, 
            IParametersParser parameterParser, 
            IServiceProvider serviceProvider, 
            IExecutorFactory executorFactory)
        {
            _methodStorage = methodStorage;
            _updateContext = updateContextAccessor.UpdateContext;
            _stateStorage = stateStorage;
            _parameterParserOptions = parameterParserOptions.Value;
            _parameterParser = parameterParser;
            _serviceProvider = serviceProvider;
            _executorFactory = executorFactory;
        }

        public async Task<IEnumerable<ExecutedMethodMetadata>> TryExecuteMethodsForCurrentUpdateAsync()
        {
            var result = new List<ExecutedMethodMetadata>();
            var methods = await getMethodInfosToExecuteAsync();

            if (methods.Any() == false)
            {
                return result;
            }

            foreach (var method in methods)
            {
                result.Add(await tryExecuteMethodAsync(method));
            }

            return result;
        }

        private async Task<ExecutedMethodMetadata> tryExecuteMethodAsync(MethodInfo methodInfo)
        {
            var metadata = new ExecutedMethodMetadata
            {
                MethodInfo = methodInfo,
            };
            
            ValidateInputDataAttribute? failedValidateAttribute = validate(methodInfo);
            if (failedValidateAttribute != null)
            {
                metadata.FailedValidationAttribute = failedValidateAttribute;
                return metadata;
            }

            ParametersParseResult parseResult = _parameterParser.Parse(
                _updateContext,
                methodInfo,
                _parameterParserOptions.DefaultSeparator
            );

            metadata.ParametersParseResult = parseResult;
            if (parseResult.Status != ParseStatus.Success)
            {
                return metadata;
            }

            await methodInfo.InvokeMethodAsync(_executorFactory, parseResult);
            return metadata;
        }

        private ValidateInputDataAttribute? validate(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<ValidateInputDataAttribute>()
                .FirstOrDefault(attr => attr.ValidateAsync(_updateContext, _serviceProvider).Result == false);
        }

        private async Task<IEnumerable<MethodInfo>> getMethodInfosToExecuteAsync()
        {
            var userStates = await _stateStorage.GetAsync(_updateContext.TelegramUserId);

            var methods = _methodStorage.Routes.GetTargetMethodInfos(
                _updateContext.Update.Type,
                userStates
            );

            var targetMethods = methods.Where(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(_updateContext.Update))
            );

            return targetMethods.Select(m => m.MethodInfo);
        }
    }
}
