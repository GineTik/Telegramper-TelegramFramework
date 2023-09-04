using System.Reflection;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.Initialization
{
    public static class StaticExecutorFinder
    {
        private static Type _baseExecutorType = typeof(Executor);

        public static IEnumerable<Type> FindExecutorTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes().Where(isInheritedFromBaseExecutorType));
        }

        private static bool isInheritedFromBaseExecutorType(Type executorType)
        {
            return executorType != _baseExecutorType
                && _baseExecutorType.IsAssignableFrom(executorType);
        }
    }
}
