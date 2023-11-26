using System.Reflection;
using Telegramper.Executors.Initialization;

namespace Telegramper.Executors.Common.Options
{
    public class ExecutorOptions
    {
        public IEnumerable<SmartAssembly> Assemblies { get; set; } = new List<SmartAssembly>
            { new(Assembly.GetExecutingAssembly()) };
        public CommandExecutorOptions MethodNameTransformer { get; set; } = new();
        public ParameterParserOptions ParameterParser { get; set; } = new();
        public UserStateOptions UserState { get; set; } = new();
    }
}
