using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Factory;

namespace Telegramper.Executors.QueryHandlers.Models
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
