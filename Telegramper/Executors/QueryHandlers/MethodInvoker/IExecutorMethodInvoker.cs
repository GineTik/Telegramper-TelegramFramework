using Telegramper.Executors.QueryHandlers.Models;

namespace Telegramper.Executors.QueryHandlers.MethodInvoker
{
    public interface IExecutorMethodInvoker
    {
        Task InvokeAsync(IEnumerable<InvokableExecutorMethod> invokableMethods);
    }
}
