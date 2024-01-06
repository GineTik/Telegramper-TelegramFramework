using System.Reflection;
using Telegramper.Executors.QueryHandlers.Extensions;
using Telegramper.Executors.QueryHandlers.ParameterParser.Converters;
using Telegramper.Executors.QueryHandlers.ParameterParser.Enums;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;

public class DefaultParseStrategy : IParametersParseStrategy
{
    private static readonly List<IParameterTypeConverter> Converters = new();

    public DefaultParseStrategy(BasicTypeConvertor basicTypeConvertor)
    {
        Converters.Add(basicTypeConvertor);
    }
    
    public ParseStatus TryParse(ICollection<string> args, ICollection<ParameterInfo> parameterInfos, 
        out object?[] convertedArgs)
    {
        convertedArgs = Array.Empty<object>();
        
        if (args.Count == 0)
        {
            return parameterInfos.Count == 0 ? ParseStatus.Success : ParseStatus.ArgsLengthIsLess;
        }
        
        var argsAsQueue = new Queue<string>(args);

        var numberOfMandatoryParameters = parameterInfos.Count - parameterInfos.NullableCount();
        var isCorrectArgsCount = argsAsQueue.Count >= numberOfMandatoryParameters;

        if (isCorrectArgsCount == false)
        {
            return ParseStatus.ArgsLengthIsLess;
        }

        var unmandatoryParametersThatCanBeFilledIn = argsAsQueue.Count - numberOfMandatoryParameters;

        try
        {
            convertedArgs = parse(parameterInfos, unmandatoryParametersThatCanBeFilledIn, argsAsQueue).ToArray();
        }
        catch
        {
            return ParseStatus.ParseError;
        }
        
        return ParseStatus.Success;
    }

    private static IEnumerable<object?> parse(IEnumerable<ParameterInfo> parameterInfos,
        int unmandatoryParametersThatCanBeFilledIn, Queue<string> argsAsQueue)
    {
        foreach (var parameterInfo in parameterInfos)
        {
            var isNullable = parameterInfo.IsNullable();

            if (isNullable)
            {
                if (unmandatoryParametersThatCanBeFilledIn == 0)
                {
                    yield return null;
                    continue;
                }

                unmandatoryParametersThatCanBeFilledIn--;
            }

            var typeConvertor = Converters.First(c => c.CanConvert(parameterInfo.ParameterType));
            var convertedArg = typeConvertor.ConvertTo(
                parameterInfo.ParameterType,
                argsAsQueue.Dequeue(),
                isNullable
            );
            yield return convertedArg;
        }
    }
}