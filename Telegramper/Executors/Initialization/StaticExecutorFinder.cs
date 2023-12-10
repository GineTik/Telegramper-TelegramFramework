using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.Initialization
{
    public static class StaticExecutorFinder
    {
        private static readonly Type BaseExecutorType = typeof(Executor);

        public static IEnumerable<ExecutorType> FindExecutorTypes(IEnumerable<SmartAssembly> assemblies)
        {
            return assemblies
                .SelectMany(smartAssembly => smartAssembly
                    .Assembly
                    .GetTypes()
                    .Where(isInheritedFromBaseExecutorType)
                    .Select(type => new ExecutorType(type, smartAssembly.AssemblyAttributes)));
        }

        private static bool isInheritedFromBaseExecutorType(Type executorType)
        {
            return executorType != BaseExecutorType
                && BaseExecutorType.IsAssignableFrom(executorType);
        }
    }
}
