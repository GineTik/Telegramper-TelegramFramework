using Telegramper.Executors.Routing.Factories.Executors;

namespace Telegramper.Executors.Routing.Models
{
    public class InvokableExecutorMethod
    {
        public ExecutorMethod Method { get; set; } = default!;
        public object?[] Parameters { get; set; } = default!;

        public async Task InvokeAsync(IExecutorFactory executorFactory)
        {
            var executor = executorFactory.CreateExecutor(Method.ExecutorType);
            await (Task)Method.MethodInfo.Invoke(executor, Parameters)!;
        }
    }
}
