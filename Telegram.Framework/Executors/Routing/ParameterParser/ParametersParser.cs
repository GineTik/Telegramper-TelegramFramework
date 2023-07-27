using Telegram.Framework.Executors.Helpers.Extensions.Nullable;
using System.Reflection;
using System.Text.RegularExpressions;
using Telegram.Framework.Executors.Routing.ParametersParser.Results;

namespace Telegram.Framework.Executors.Routing.ParametersParser
{
    public class ParametersParser : IParametersParser
    {
        private int _missingArgs;

        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator)
        {
            var args = getArgsAsStack(text, parameterSeparator);
            var parametersStack = new Stack<ParameterInfo>(parameters);

            if (isCorrectArgsCount(args, parametersStack))
                return ParametersParseResult.ArgsLengthIsLess;

            _missingArgs = parametersStack.Count - args.Count;
            var convertedArgs = new Stack<object?>();

            try
            {
                while (parametersStack.Count != 0)
                {
                    var value = convertArgsToMethodParameter(args, parametersStack);
                    convertedArgs.Push(value);
                }
            }
            catch
            {
                return ParametersParseResult.ParseError;
            }

            return ParametersParseResult.Success(convertedArgs.ToArray());
        }

        private static Stack<string> getArgsAsStack(string text, string parameterSeparator)
        {
            text = Regex.Replace(text ?? "", "^/*\\w+\\s*", "");
            return string.IsNullOrEmpty(text) ?
                new Stack<string>() :
                new Stack<string>(text.Split(parameterSeparator));
        }

        private bool isCorrectArgsCount(Stack<string> args, Stack<ParameterInfo> parameters)
        {
            return parameters.Count - parameters.NullableCount() > args.Count;
        }

        private object? convertArgsToMethodParameter(Stack<string> args, Stack<ParameterInfo> parameters)
        {
            var parameter = parameters.Pop();
            var targetType = parameter.ParameterType;

            if (parameter.IsNullable() == true && _missingArgs != 0)
            {
                _missingArgs--;
                return null;
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                targetType = Nullable.GetUnderlyingType(targetType)!;
            }

            var stringValue = args.Pop();

            return Convert.ChangeType(stringValue, targetType);
        }
    }
}
