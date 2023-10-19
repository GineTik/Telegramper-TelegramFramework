using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.Initialization
{
    public static class StaticExecutorFinder
    {
        private static readonly Type _baseExecutorType = typeof(Executor);

        public static IEnumerable<ExecutorTypeWrapper> FindExecutorTypes(IEnumerable<SmartAssembly> assemblies)
        {
            return assemblies.SelectMany(smartAssembly => smartAssembly.Assembly.GetTypes().Where(isInheritedFromBaseExecutorType).Select(type => new ExecutorTypeWrapper(type, smartAssembly.GlobalAttributes)));
        }

        private static bool isInheritedFromBaseExecutorType(Type executorType)
        {
            return executorType != _baseExecutorType
                && _baseExecutorType.IsAssignableFrom(executorType);
        }
    }
}
