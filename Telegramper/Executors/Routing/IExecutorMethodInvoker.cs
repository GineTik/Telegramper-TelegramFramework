using Telegramper.Executors.Routing.Models;

namespace Telegramper.Executors.Routing
{
    public interface IExecutorMethodInvoker
    {
        Task InvokeAsync(IEnumerable<InvokableExecutorMethod> invokableMethods);
    }
}
