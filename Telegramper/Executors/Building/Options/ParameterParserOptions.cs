using Telegramper.Executors.Building.Exceptions;
using Telegramper.Executors.Routing.Attributes.ParametersParse;
using Telegramper.Executors.Routing.ParametersParser;

namespace Telegramper.Executors.Build.Options
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
