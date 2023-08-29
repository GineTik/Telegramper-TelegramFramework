using Microsoft.Extensions.Options;
using System.Reflection;
using Telegramper.Executors.Build.Options;
using Telegramper.Executors.Routing.Attributes.ParametersParse;
using Telegramper.Executors.Routing.Models;
using Telegramper.Executors.Routing.ParametersParser;
using Telegramper.Executors.Routing.ParametersParser.Extensions;
using Telegramper.Executors.Routing.ParametersParser.Results;
using Telegramper.Executors.Routing.Preparer;
using Telegramper.Executors.Routing.RoutesStorage;
using Telegramper.Executors.Routing.UserState;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.Executors.Routing.Middleware.TargetExecutor
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ISuitableMethodFinder _suitableMethodFinder;
        private readonly IExecutorMethodInvoker _executorMethodInvoker;
        private readonly IParametersParser _parametersParser;
        private readonly IExecutorMethodPreparer _executorMethodPreparer;
        private readonly UpdateContext _updateContext;
        private readonly ParameterParserOptions _parameterParserOptions;

        public TargetExecutorMiddleware(
            IOptions<ParameterParserOptions> parameterParserOptions,
            IExecutorMethodInvoker executorInvoker,
            IParametersParser parametersParser,
            UpdateContextAccessor updateContextAccessor,
            ISuitableMethodFinder suitableMethodFinder,
            IExecutorMethodPreparer executorMethodPreparer)
        {
            _parameterParserOptions = parameterParserOptions.Value;
            _executorMethodInvoker = executorInvoker;
            _parametersParser = parametersParser;
            _updateContext = updateContextAccessor.UpdateContext;
            _suitableMethodFinder = suitableMethodFinder;
            _executorMethodPreparer = executorMethodPreparer;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var suitableMethods = await _suitableMethodFinder.FindSuitableMethodsForCurrentUpdateAsync();
            var invokableMethods = _executorMethodPreparer.PrepareMethodsForExecution(suitableMethods, out var errors);

            foreach (var error in errors)
            {
                await _updateContext.Client.SendTextMessageAsync(error.Message);
            }

            if (invokableMethods.Any() == false)
            {
                await next();
                return;
            }

            await _executorMethodInvoker.InvokeAsync(invokableMethods);
        }

        //private async Task<IEnumerable<InvokableExecutorMethod>> prepareMethodsForExecution(IEnumerable<ExecutorMethod> suitableMethods)
        //{
        //    var invokableMethods = new List<InvokableExecutorMethod>();
        //    foreach (var suitableMethod in suitableMethods)
        //    {
        //        var parseResult = _parametersParser.Parse(
        //            _updateContext,
        //            suitableMethod.MethodInfo,
        //            _parameterParserOptions.DefaultSeparator
        //        );

        //        if (parseResult == null)
        //        {
        //            continue;
        //        }

        //        if (parseResult.Status != ParseStatus.Success)
        //        {
        //            await handleParseErrorAsync(suitableMethod.MethodInfo, parseResult.Status);
        //            continue;
        //        }

        //        invokableMethods.Add(new InvokableExecutorMethod
        //        {
        //            Method = suitableMethod,
        //            Parameters = parseResult.ConvertedParameters
        //        });
        //    }
        //    return invokableMethods;
        //}

        private async Task handleParseErrorAsync(MethodInfo methodInfo, ParseStatus parseStatus)
        {
            var parseErrorMessages =
                methodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>() ??
                _parameterParserOptions.ErrorMessages;

            var errorMessage = parseErrorMessages.GetActualErrorMessage(parseStatus);
            await _updateContext.Client.SendTextMessageAsync(errorMessage ?? throw new InvalidOperationException("Error message missed"));
        }
    }
}
