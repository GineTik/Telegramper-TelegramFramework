using Telegram.Framework.Attributes.ParametersParse;
using Telegram.Framework.Executors.Routing.ParametersParser;
using Telegram.Framework.Executors.Storages.UserState.Saver.Implementations;

namespace Telegram.Framework.Executors.Configuration.Options
{
    public class ExecutorOptions
    {
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
