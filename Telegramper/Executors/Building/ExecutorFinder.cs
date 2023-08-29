using System.Reflection;
using Telegramper.Executors.Building.NameTransformer;

namespace Telegramper.Executors.Building
{
    public static class ExecutorFinder
    {
        private static Type _baseExecutorType = typeof(Executor);

        public static IEnumerable<Type> FindExecutorTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(assembly => assembly.GetTypes().Where(isInheritedFromBaseExecutorType));
        }

        public static IEnumerable<ExecutorMethod> FindExecutorsMethods(IEnumerable<Type> executorTypes, IServiceProvider serviceProvider)
        {
            foreach (var executorType in executorTypes)
            {
                foreach (MethodInfo methodInfo in executorType.GetMethods())
                {
                    var executorMethod = new ExecutorMethod(methodInfo, serviceProvider);
                    if (executorMethod.TargetAttributes.Any() == true)
                    {
                        yield return executorMethod;
                    }
                }
            }
        }

        private static bool isInheritedFromBaseExecutorType(Type executorType)
        {
            return executorType != _baseExecutorType
                && _baseExecutorType.IsAssignableFrom(executorType);
        }
    }
}
