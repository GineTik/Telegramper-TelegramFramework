using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;

public interface IParseErrorStrategy
{
    string Handle(string errorMessage, IEnumerable<object?> convertedArgs, Route route);
}