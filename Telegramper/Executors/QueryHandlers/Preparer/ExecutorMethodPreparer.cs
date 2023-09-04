using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.Executors.QueryHandlers.ParametersParser;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;
using Telegramper.Executors.QueryHandlers.Preparer.PrepareErrors;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.Executors.QueryHandlers.ParametersParser.Extensions;
using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.Preparer
{
    public class ExecutorMethodPreparer : IExecutorMethodPreparer
    {
        private readonly IParametersParser _parametersParser;
        private readonly UpdateContext _updateContext;
        private readonly ParameterParserOptions _parameterParserOptions;

        public ExecutorMethodPreparer(
            IParametersParser parametersParser,
            UpdateContextAccessor updateContextAccessor,
            IOptions<ParameterParserOptions> parameterParserOptions)
        {
            _parametersParser = parametersParser;
            _updateContext = updateContextAccessor.UpdateContext;
            _parameterParserOptions = parameterParserOptions.Value;
        }

        public IEnumerable<InvokableExecutorMethod> PrepareMethodsForExecution(
            IEnumerable<ExecutorMethod> methods, 
            out IEnumerable<PrepareError> prepareErrors)
        {
            var prepareErrorsList = new List<PrepareError>();
            var invokableMethods = new List<InvokableExecutorMethod>();

            foreach (var method in methods)
            {
                var parseResult = _parametersParser.Parse(
                    _updateContext,
                    method.MethodInfo,
                    _parameterParserOptions.DefaultSeparator
                );

                if (parseResult == null)
                {
                    continue;
                }

                if (parseResult.Status != ParseStatus.Success)
                {
                    prepareErrorsList.Add(new PrepareError
                    {
                        Method = method,
                        Message = parseResult.Status.ToString() // not completed
                    });
                    continue;
                }

                invokableMethods.Add(new InvokableExecutorMethod
                {
                    Method = method,
                    Parameters = parseResult.ConvertedParameters
                });
            }

            prepareErrors = prepareErrorsList;
            return invokableMethods;
        }
    }
}
