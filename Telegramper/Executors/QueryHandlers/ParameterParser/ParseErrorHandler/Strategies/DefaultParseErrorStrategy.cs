using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;

public class DefaultParseErrorStrategy : IParseErrorStrategy
{
    public string Handle(string errorMessage, IEnumerable<object?> convertedArgs, Route route)
    {
        return errorMessage;
    }
}