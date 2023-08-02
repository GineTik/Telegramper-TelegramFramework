using Telegramper.Executors.Attributes.ParametersParse;
using Telegramper.Executors.NameTransformer;
using Telegramper.Executors.Routing.ParametersParser;
using Telegramper.Executors.Storages.UserState.Saver.Implementations;

namespace Telegramper.Executors.Configuration.Options
{
    public class ExecutorOptions
    {
        public CommandExecutorOptions MethodNameTransformer { get; set; } = new CommandExecutorOptions
        {
            Type = typeof(SnakeCaseNameTransformer)
        };

        public ParameterParserOptions ParameterParser { get; set; } = new ParameterParserOptions
        {
            DefaultSeparator = " ",
            ParserType = typeof(ParametersParser),
            ErrorMessages = new ParseErrorMessagesAttribute()
            {
                TypeParseError = "Parse type error",
                ArgsLengthIsLess = "Args length is less"
            }
        };

        public UserStateOptions UserState { get; set; } = new UserStateOptions
        {
            DefaultUserState = "",
            SaverType = typeof(MemoryUserStateSaver),
        };
    }
}
