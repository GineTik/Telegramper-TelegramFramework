using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.Executors.QueryHandlers.ParametersParser;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;
using Telegramper.Executors.QueryHandlers.Preparer.PrepareErrors;
using Telegramper.Core.Context;
using Telegramper.Executors.QueryHandlers.ParametersParser.Extensions;
using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Preparer.ErrorHandler;

namespace Telegramper.Executors.QueryHandlers.Preparer
{
    public class ExecutorMethodPreparer : IExecutorMethodPreparer
    {
        private readonly IParametersParser _parametersParser;
        private readonly UpdateContext _updateContext;
        private readonly ParametersParserOptions _parametersParserOptions;
        private readonly IParseErrorHandler _parseErrorHandler;

        public ExecutorMethodPreparer(
            IParametersParser parametersParser,
            UpdateContextAccessor updateContextAccessor,
            IOptions<ParametersParserOptions> parameterParserOptions,
            IParseErrorHandler parseErrorHandler)
        {
            _parametersParser = parametersParser;
            _updateContext = updateContextAccessor.UpdateContext;
            _parametersParserOptions = parameterParserOptions.Value;
            _parseErrorHandler = parseErrorHandler;
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
                    _parametersParserOptions.DefaultSeparator
                );

                if (parseResult.Status != ParseStatus.Success) 
                {
                    var errorMessage = _parseErrorHandler.GetErrorMessage(parseResult.Status, method);
                    prepareErrorsList.Add(new PrepareError
                    {
                        Method = method,
                        Message = errorMessage
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
