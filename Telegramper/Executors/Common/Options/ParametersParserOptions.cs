using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParametersParser;

namespace Telegramper.Executors.Common.Options
{
    public class ParametersParserOptions
    {
        /// default values see in <see cref="ExecutorOptions"/>

        public static string NoneSeparator = "";
        
        private Type _parametersParserType = null!;

        public required Type ParserType
        {
            get => _parametersParserType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParametersParser>(value);
                _parametersParserType = value;
            }
        }
        
        public required string DefaultSeparator { get; set; }

        public required ParseErrorMessagesAttribute ErrorMessages { get; set; }
    }
}
