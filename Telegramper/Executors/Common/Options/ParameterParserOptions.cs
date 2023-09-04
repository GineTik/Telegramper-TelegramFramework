using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParametersParser;

namespace Telegramper.Executors.Common.Options
{
    public class ParameterParserOptions
    {
        private Type _parametersParserType = typeof(ParametersParser);
        public Type ParserType
        {
            get
            {
                return _parametersParserType;
            }
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParametersParser>(value);
                _parametersParserType = value;
            }
        }
        public string DefaultSeparator { get; set; } = " ";
        public ParseErrorMessagesAttribute ErrorMessages { get; set; } = new ParseErrorMessagesAttribute
        {
            TypeParseError = "Type parse error",
            ArgsLengthIsLess = "Args length is less"
        };
    }
}
