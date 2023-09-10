using Telegramper.Core.Context;
using System.Reflection;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse.Separator;

namespace Telegramper.Executors.QueryHandlers.ParametersParser.Extensions
{
    public static class ParameterParserExtensions
    {
        public static ParametersParseResult Parse(
            this IParametersParser parser, 
            UpdateContext actual,
            MethodInfo methodInfo, 
            string defaultSeparator)
        {
            var text = getTextWithArgs(actual);
            var parameters = methodInfo.GetParameters();
            var separator = getSeparator(methodInfo, defaultSeparator);

            return parser.Parse(
                text,
                parameters,
                separator
            );
        }

        private static string getTextWithArgs(UpdateContext updateContext)
        {
            return
                updateContext.Update.Message?.Text ??
                updateContext.Update.CallbackQuery?.Data ??
                "";
        }

        private static string getSeparator(MethodInfo methodInfo, string defaultSeparator)
        {
            return
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                defaultSeparator;
        }
    }
}
