using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Framework.Attributes.ParametersParse;
using Telegram.Framework.Executors.Configuration.Options;
using Telegram.Framework.Executors.Helpers.Extensions.MethodInfos;
using Telegram.Framework.Executors.Helpers.Factories.Executors;
using Telegram.Framework.Executors.Parsers.ExecutorParameters;
using Telegram.Framework.Executors.Parsers.ExecutorParameters.Extensions;
using Telegram.Framework.Executors.Parsers.ExecutorParameters.Results;
using Telegram.Framework.Executors.Storages.TargetMethod;
using Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Framework.TelegramBotApplication.Configuration.Middlewares;
using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Framework.TelegramBotApplication.Delegates;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Telegram.Framework.Executors.Configuration.Middleware.TargetExecutor
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IParametersParser _parameterParser;
        private readonly ParameterParserOptions _parameterParserOptions;
        private readonly IServiceProvider _provider;

        public TargetExecutorMiddleware(IOptions<ParameterParserOptions> parameterParserOptions, IParametersParser parameterParser,
            IExecutorFactory executorFactory, ITargetMethodStorage methodStorage, IServiceProvider provider)
        {
            _parameterParserOptions = parameterParserOptions.Value;
            _parameterParser = parameterParser;
            _executorFactory = executorFactory;
            _methodStorage = methodStorage;
            _provider = provider;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            MethodInfo? methodInfo = await _methodStorage.GetMethodInfoToExecuteAsync(updateContext);

            if (methodInfo == null)
            {
                await next();
                return;
            }

            var failedValidateAttribute = methodInfo.GetCustomAttributes<ValidateInputDataAttribute>()
                .FirstOrDefault(attr => attr.ValidateAsync(updateContext, _provider).Result == false);

            if (failedValidateAttribute != null)
            {
                await updateContext.Client.SendTextMessageAsync(failedValidateAttribute.ErrorMessage);
                return;
            }

            ParametersParseResult parseResult = _parameterParser.Parse(
                updateContext,
                methodInfo,
                _parameterParserOptions.DefaultSeparator
            );

            if (parseResult.Status != ParseStatus.Success)
            {
                await handleParseErrorAsync(updateContext, methodInfo!, parseResult.Status);
                return;
            }

            await methodInfo.InvokeMethodAsync(_executorFactory, parseResult);
        }

        private async Task handleParseErrorAsync(UpdateContext updateContext, MethodInfo methodInfo, ParseStatus parseStatus)
        {
            var parseErrorMessages =
                methodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>() ??
                _parameterParserOptions.ErrorMessages;

            var errorMessage = parseErrorMessages.GetActualErrorMessage(parseStatus);
            await updateContext.Client.SendTextMessageAsync(errorMessage ?? parseStatus.ToString());
        }
    }
}
