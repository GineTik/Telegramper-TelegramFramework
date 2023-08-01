using Telegramper.Executors.Helpers.Factories.Executors;
using System.Reflection;
using Telegramper.Executors.Routing.ParametersParser.Results;

namespace Telegramper.Executors.Helpers.Extensions.MethodInfos
{
    internal static class InvokeMethodExtensions
    {
        public static async Task InvokeMethodAsync(this MethodInfo methodInfo, IExecutorFactory factory, ParametersParseResult parseResult)
        {
            var executorType = getExecutorType(methodInfo);
            var executor = factory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parseResult.ConvertedParameters)!;
        }

        private static Type getExecutorType(MethodInfo methodInfo)
        {
            return
                methodInfo.DeclaringType ??
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType and ReflectedType");
        }
    }
}
