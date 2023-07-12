using Telegram.Framework.Executors.Parsers.ExecutorParameters.Results;

namespace Telegram.Framework.Attributes.ParametersParse
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParseErrorMessagesAttribute : Attribute
    {
        public string? ArgsLengthIsLess { get; set; }
        public string? TypeParseError { get; set; }

        public string? GetActualErrorMessage(ParseStatus status)
        {
            return status switch
            {
                ParseStatus.ParseError => TypeParseError,
                ParseStatus.ArgsLengthIsLess => ArgsLengthIsLess,
                _ => null
            };
        }
    }
}
