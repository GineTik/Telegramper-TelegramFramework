using Telegram.Framework.Attributes.ParametersParse;
using Telegram.Framework.Executors.Helpers.Exceptions;
using Telegram.Framework.Executors.Parsers.ExecutorParameters;

namespace Telegram.Framework.Executors.Configuration.Options
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
