namespace Telegramper.Executors.Build.Options
{
    public class ExecutorOptions
    {
        public CommandExecutorOptions MethodNameTransformer { get; set; } = new CommandExecutorOptions();
        public ParameterParserOptions ParameterParser { get; set; } = new ParameterParserOptions();
        public UserStateOptions UserState { get; set; } = new UserStateOptions();
    }
}
