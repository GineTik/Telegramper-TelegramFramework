using Microsoft.Extensions.Options;
using System.Reflection;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;

namespace Telegramper.Executors.QueryHandlers.Preparer.ErrorHandler
{
    public class ParseErrorHandler : IParseErrorHandler
    {
        private readonly ParametersParserOptions _parseOptions;

        public ParseErrorHandler(
            IOptions<ParametersParserOptions> parseOptions)
        {
            _parseOptions = parseOptions.Value;
        }

        public string GetErrorMessage(ParseStatus status, ExecutorMethod method)
        {
            return method.MethodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>()?.GetActualErrorMessage(status)
                ?? _parseOptions.ErrorMessages.GetActualErrorMessage(status)
                ?? status.ToString();
        }
    }
}
