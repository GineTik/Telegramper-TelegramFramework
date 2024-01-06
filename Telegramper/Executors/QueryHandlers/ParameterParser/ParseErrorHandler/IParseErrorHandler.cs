using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.ParameterParser.Enums;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler;

public interface IParseErrorHandler
{
    string Handle(ParseStatus status, IEnumerable<object?> convertedArgs, Route route);
}