using System.Reflection;
using Telegram.Framework.Executors.Routing.Models;

namespace Telegram.Framework.Executors.Routing
{
    public interface IExecutorRouter
    {
        Task<IEnumerable<ExecutedMethodMetadata>> TryExecuteMethodsForCurrentUpdateAsync();
    }
}
