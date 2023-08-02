using Telegramper.TelegramBotApplication.Context;
using System.Reflection;
using Telegramper.Executors.Routing.ParametersParser.Results;
using Telegramper.Executors.Attributes.ParametersParse.Separator;

namespace Telegramper.Executors.Routing.ParametersParser.Extensions
{
    public static class ParameterParserExtensions
    {
        public static ParametersParseResult Parse(this IParametersParser parser, UpdateContext actual,
            MethodInfo method, string defaultSeparator)
        {
            var text = getTextWithArgs(actual);
            var parameters = method.GetParameters();
            var separator = getSeparator(method, defaultSeparator);

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
