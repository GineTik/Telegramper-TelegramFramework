using Microsoft.Extensions.Options;
using Telegramper.Core.Context;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.Executors.QueryHandlers.ParameterParser;
using Telegramper.Executors.QueryHandlers.Preparer.Models;

namespace Telegramper.Executors.QueryHandlers.Preparer
{
    public class ExecutorMethodPreparer : IExecutorMethodPreparer
    {
        private readonly IParametersParser _parametersParser;

        public ExecutorMethodPreparer(
            IParametersParser parametersParser)
        {
            _parametersParser = parametersParser;
        }

        public IEnumerable<InvokableExecutorMethod> PrepareMethodsForExecution(
            IEnumerable<ExecutorMethod> methods, 
            out IEnumerable<PrepareError> prepareErrors)
        {
            var prepareErrorsAsList = new List<PrepareError>();
            var invokableMethods = new List<InvokableExecutorMethod>();

            foreach (var method in methods)
            {
                var parseResult = _parametersParser.TryParse(method);

                if (parseResult.Successfully == false) 
                {
                    prepareErrorsAsList.Add(new PrepareError
                    {
                        Method = method,
                        Message = parseResult.ErrorMessage!
                    });
                    continue;
                }

                invokableMethods.Add(new InvokableExecutorMethod
                {
                    Method = method,
                    Parameters = parseResult.ConvertedParameters
                });
            }

            prepareErrors = prepareErrorsAsList;
            return invokableMethods;
        }
    }
}
