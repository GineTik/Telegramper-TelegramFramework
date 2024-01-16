using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParameterParser.Enums;
using Telegramper.Executors.QueryHandlers.ParameterParser.Models;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler;

public class ParseErrorHandler : IParseErrorHandler
{
    private readonly ParametersParserOptions _parametersParserOptions;
    private readonly IParseErrorStrategy _parseErrorStrategy;

    public ParseErrorHandler(IOptions<ParametersParserOptions> parametersParserOptions, IParseErrorStrategy parseErrorStrategy)
    {
        _parseErrorStrategy = parseErrorStrategy;
        _parametersParserOptions = parametersParserOptions.Value;
    }

    public string Handle(ParseStatus status, IEnumerable<object?> convertedArgs, Route route)
    {
        var errorMessage = route.Method.GetCustomAttribute<ParseErrorMessagesAttribute>()?.GetActualErrorMessage(status)
            ?? _parametersParserOptions.ErrorMessages.GetActualErrorMessage(status)
            ?? status.ToString();
        
        return _parseErrorStrategy.Handle(errorMessage, convertedArgs, route);
    }
}