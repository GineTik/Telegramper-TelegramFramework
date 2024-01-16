using System.Reflection;
using Telegramper.Executors.QueryHandlers.ParameterParser.Enums;
using Telegramper.Executors.QueryHandlers.ParameterParser.Models;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;

public interface IParametersParseStrategy
{
    ParseStatus TryParse(ICollection<string> args, ICollection<ParameterInfo> parameterInfos, out object?[] convertedArgs);
}