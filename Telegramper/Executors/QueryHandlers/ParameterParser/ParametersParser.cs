using System.Reflection;
using Telegramper.Executors.QueryHandlers.Extensions;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParameterTypeConvertors;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;

namespace Telegramper.Executors.QueryHandlers.ParametersParser
{
    public class ParametersParser : IParametersParser
    {
        public ParametersParseResult Parse(string args, ICollection<ParameterInfo> parametersInfos, string argsSeparator)
        {
            if (args == "")
            {
                if (parametersInfos.Count == 0)
                {
                    return ParametersParseResult.Success(new string[0]);
                }
                else
                {
                    return ParametersParseResult.ArgsLengthIsLess;
                }
            }

            var argsAsQueue = new Queue<string>(args.Split(argsSeparator));

            int numberOfMandatoryParameters = parametersInfos.Count - parametersInfos.NullableCount();
            bool isCorrectArgsCount = argsAsQueue.Count >= numberOfMandatoryParameters;

            if (isCorrectArgsCount == false)
            {
                return ParametersParseResult.ArgsLengthIsLess;
            }

            var unmandatoryParametersThatCanBeFilledIn = argsAsQueue.Count - numberOfMandatoryParameters;
            var convertedArgs = new List<object?>();

            foreach (var parameterInfo in parametersInfos)
            {
                var isNullable = parameterInfo.IsNullable();

                if (isNullable == true)
                {
                    if (unmandatoryParametersThatCanBeFilledIn == 0)
                    {
                        convertedArgs.Add(null);
                        continue;
                    }

                    unmandatoryParametersThatCanBeFilledIn--;
                }

                try
                {
                    var typeConvertor = new BasicTypeConvertor();
                    var convertedArg = typeConvertor.ConvertTo(
                        parameterInfo.ParameterType, 
                        argsAsQueue.Dequeue(), 
                        isNullable
                    );
                    convertedArgs.Add(convertedArg);
                }
                catch
                {
                    return ParametersParseResult.ParseError;
                }
            }

            return ParametersParseResult.Success(convertedArgs.ToArray());
        }
    }
}
