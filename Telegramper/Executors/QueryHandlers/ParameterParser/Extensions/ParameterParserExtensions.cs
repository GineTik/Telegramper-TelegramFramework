using Telegramper.Core.Context;
using System.Reflection;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse.Separator;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;
using Telegramper.Executors.QueryHandlers.ParameterParser.Extensions;

namespace Telegramper.Executors.QueryHandlers.ParametersParser.Extensions
{
    public static class ParameterParserExtensions
    {
        public static ParametersParseResult Parse(
            this IParametersParser parser, 
            UpdateContext actualUpdateContext,
            MethodInfo methodInfo, 
            string defaultSeparator)
        {
            var text = takeArgs(actualUpdateContext.Update);
            var parameters = methodInfo.GetParameters();
            var separator = takeSeparator(methodInfo, defaultSeparator);

            return parser.Parse(
                text,
                parameters,
                separator
            );
        }

        private static string takeArgs(Update udpate)
        {
            return
                udpate.Message?.Text?.RemoveCommand() ??
                udpate.CallbackQuery?.Data?.RemoveFirstTargetCallbackData() ??
                "";
        }

        private static string takeSeparator(MethodInfo methodInfo, string defaultSeparator)
        {
            return
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                defaultSeparator;
        }
    }
}
