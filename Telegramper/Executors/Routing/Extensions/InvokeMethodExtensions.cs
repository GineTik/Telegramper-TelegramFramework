using Telegramper.Executors.Routing.Factories.Executors;

namespace Telegramper.Executors.Routing.Extensions
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
