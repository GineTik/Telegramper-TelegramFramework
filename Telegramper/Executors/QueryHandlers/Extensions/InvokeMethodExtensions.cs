using System.Reflection;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Factory;

namespace Telegramper.Executors.QueryHandlers.Extensions
{
    internal static class InvokeMethodExtensions
    {
        public static async Task InvokeMethodAsync(
            this ExecutorMethod method,
            IExecutorFactory factory,
            object?[] parameters)
        {
            await method.MethodInfo.InvokeMethodAsync(factory, parameters);
        }
        
        public static async Task InvokeMethodAsync(
            this MethodInfo method,
            IExecutorFactory factory,
            object?[] parameters)
        {
            var executor = factory.CreateExecutor(
                method.DeclaringType ??
                method.ReflectedType ??
                throw new InvalidOperationException($"Method {method.Name} don't have DeclaringType and ReflectedType"));
            
            await (Task)method.Invoke(executor, parameters)!;
        }
    }
}
