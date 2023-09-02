using Telegramper.Executors.Routing.Models;
using Telegramper.Executors.Routing.Preparer.PrepareErrors;

namespace Telegramper.Executors.Routing.Preparer
{
    public interface IExecutorMethodPreparer
    {
        IEnumerable<InvokableExecutorMethod> PrepareMethodsForExecution(IEnumerable<ExecutorMethod> methods, out IEnumerable<PrepareError> prepareErrors);
    }
}
