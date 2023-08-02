using System.Reflection;
using Telegramper.Executors.Routing.Models;

namespace Telegramper.Executors.Routing
{
    public interface IExecutorRouter
    {
        Task<IEnumerable<ExecutedMethodMetadata>> TryExecuteMethodsForCurrentUpdateAsync();
    }
}
