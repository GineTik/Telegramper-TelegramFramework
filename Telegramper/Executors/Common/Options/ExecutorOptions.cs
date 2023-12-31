using System.Reflection;
using Telegramper.Executors.Initialization;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.QueryHandlers.Attributes.ParametersParse;
using Telegramper.Executors.QueryHandlers.ParameterParser;
using Telegramper.Executors.QueryHandlers.ParameterParser.ParseErrorHandler.Strategies;
using Telegramper.Executors.QueryHandlers.ParameterParser.Strategies;
using Telegramper.Executors.QueryHandlers.UserState.Saver.Implementations;

namespace Telegramper.Executors.Common.Options
{
    public class ExecutorOptions
    {
        public IEnumerable<SmartAssembly> Assemblies { get; set; } = new[]
        {
            new SmartAssembly(Assembly.GetEntryAssembly()!)
        };
        
        public CommandExecutorOptions MethodNameTransformer { get; set; } = new()
        {
            NameTransformerType = typeof(SnakeCaseNameTransformer)
        };

        public ParametersParserOptions ParametersParser { get; set; } = new()
        {
            ParserType = typeof(ParametersParser),
            ErrorHandlerStrategyType = typeof(DefaultParseErrorStrategy),
            DefaultSeparator = ParametersParserOptions.NoneSeparator,
            ErrorMessages = new ParseErrorMessagesAttribute
            {
                TypeParseError = "Type parse error",
                ArgsLengthIsLess = "Args length is less"
            },
            ParameterParseStrategyType = typeof(DefaultParseStrategy)
        };
            
        public UserStateOptions UserState { get; set; } = new()
        {
            DefaultUserState = "",
            SaverType = typeof(MemoryUserStateSaver)
        };

        public HandlerQueueOptions HandlerQueue { get; set; } = new()
        {
            LimitOfHandlersPerRequest = 1
        };
    }
}
