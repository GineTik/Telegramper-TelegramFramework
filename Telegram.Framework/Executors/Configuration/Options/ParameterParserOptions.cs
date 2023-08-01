using Telegramper.Attributes.ParametersParse;
using Telegramper.Executors.Helpers.Exceptions;
using Telegramper.Executors.Routing.ParametersParser;

namespace Telegramper.Executors.Configuration.Options
{
    public class ParameterParserOptions
    {
        private Type _parametersParserType;
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
        public string DefaultSeparator { get; set; }
        public ParseErrorMessagesAttribute ErrorMessages { get; set; } = new ParseErrorMessagesAttribute();
    }
}
