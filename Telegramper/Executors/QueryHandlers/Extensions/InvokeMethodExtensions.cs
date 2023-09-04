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
            var executor = factory.CreateExecutor(method.ExecutorType);
            await (Task)method.MethodInfo.Invoke(executor, parameters)!;
        }
    }
}
