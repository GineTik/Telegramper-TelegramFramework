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

            //var args = getArgsAsStack(text, argsSeparator);
            //var parametersStack = new Stack<ParameterInfo>(args);

            //if (isCorrectArgsCount(args, parametersStack))
            //{ 
            //    return ParametersParseResult.ArgsLengthIsLess;
            //}

            //_missingArgs = parametersStack.Count - args.Count;
            //var convertedArgs = new Stack<object?>();

            //try
            //{
            //    while (parametersStack.Count != 0)
            //    {
            //        var value = convertArgsToMethodParameter(args, parametersStack);
            //        convertedArgs.Push(value);
            //    }
            //}
            //catch
            //{
            //    return ParametersParseResult.ParseError;
            //}

            //return ParametersParseResult.Success(convertedArgs.ToArray());
        }

        //private static Stack<string> getArgsAsStack(string text, string parameterSeparator)
        //{
        //    text = Regex.Replace(text ?? "", "^/*\\w+\\s*", ""); // remove command
        //    return string.IsNullOrEmpty(text) ?
        //        new Stack<string>() :
        //        new Stack<string>(text.Split(parameterSeparator));
        //}

        //private bool isCorrectArgsCount(ICollection<string> args, ICollection<ParameterInfo> parameterTypes)
        //{
        //    var numberOfMandatoryParameters = parameterTypes.Count - parameterTypes.NullableCount();
        //    return numberOfMandatoryParameters > args.Count;
        //}

        //private object? convertArgsToMethodParameter(Stack<string> args, Stack<ParameterInfo> parameters)
        //{
        //    var parameter = parameters.Pop();
        //    var targetType = parameter.ParameterType;

        //    if (parameter.IsNullable() == true && _missingArgs != 0)
        //    {
        //        _missingArgs--;
        //        return null;
        //    }

        //    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        //    {
        //        targetType = Nullable.GetUnderlyingType(targetType)!;
        //    }

        //    var stringValue = args.Pop();

        //    return Convert.ChangeType(stringValue, targetType);
        //}
    }
}
