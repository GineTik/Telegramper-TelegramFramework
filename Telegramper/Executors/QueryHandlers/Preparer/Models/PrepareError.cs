using Telegramper.Executors.Common.Models;

namespace Telegramper.Executors.QueryHandlers.Preparer.Models
{
    public class PrepareError
    {
        public ExecutorMethod Method { get; set; } = default!;
        public string? Message { get; set; } = default!;
    }
}
