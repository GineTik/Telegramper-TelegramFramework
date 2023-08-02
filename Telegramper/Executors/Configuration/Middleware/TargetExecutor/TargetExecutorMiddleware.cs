using Microsoft.Extensions.Options;
using System.Reflection;
using Telegramper.Executors.Attributes.ParametersParse;
using Telegramper.Executors.Configuration.Options;
using Telegramper.Executors.Routing;
using Telegramper.Executors.Routing.ParametersParser.Results;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.Executors.Configuration.Middleware.TargetExecutor
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly IExecutorRouter _executorRouter;
        private readonly ParameterParserOptions _parameterParserOptions;

        public TargetExecutorMiddleware(
            IOptions<ParameterParserOptions> parameterParserOptions,
            IExecutorRouter executorRouter)
        {
            _parameterParserOptions = parameterParserOptions.Value;
            _executorRouter = executorRouter;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var executedMethodsMetadatas = await _executorRouter.TryExecuteMethodsForCurrentUpdateAsync();

            if (executedMethodsMetadatas.Any() == false)
            {
                await next();
                return;
            }

            foreach (var executedMethodMetadata in executedMethodsMetadatas)
            {
                var failedValidateAttribute = executedMethodMetadata.FailedValidationAttribute;
                if (failedValidateAttribute != null)
                {
                    await updateContext.Client.SendTextMessageAsync(failedValidateAttribute.ErrorMessage);
                    return;
                }

                var parseResult = executedMethodMetadata.ParametersParseResult!;
                var methodInfo = executedMethodMetadata.MethodInfo!;
                if (parseResult.Status != ParseStatus.Success)
                {
                    await handleParseErrorAsync(updateContext, methodInfo, parseResult.Status);
                    return;
                }
            }
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
