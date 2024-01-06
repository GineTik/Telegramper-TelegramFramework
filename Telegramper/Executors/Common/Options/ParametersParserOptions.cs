using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParameterParser;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;
using Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;

namespace Telegramper.Executors.Common.Options
{
    public class ParametersParserOptions
    {
        /// default values see in <see cref="ExecutorOptions"/>

        public static string NoneSeparator = "";
        
        private Type _parametersParserType = null!;
        private Type _errorParserStrategyType = null!;
        private Type _parserStrategyType = null!;

        public required Type ParserType
        {
            get => _parametersParserType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParametersParser>(value);
                _parametersParserType = value;
            }
        }
        
        public required Type ErrorHandlerStrategyType
        {
            get => _errorParserStrategyType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParseErrorStrategy>(value);
                _errorParserStrategyType = value;
            }
        }
        
        public required Type ParameterParseStrategyType
        {
            get => _parserStrategyType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParametersParseStrategy>(value);
                _parserStrategyType = value;
            }
        }
        
        public required string DefaultSeparator { get; set; }

        public required ParseErrorMessagesAttribute ErrorMessages { get; set; }
    }
}
