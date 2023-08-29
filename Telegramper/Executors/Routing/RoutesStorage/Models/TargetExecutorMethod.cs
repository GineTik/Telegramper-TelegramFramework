using Telegramper.Executors.Routing.Attributes.BaseAttributes;

namespace Telegramper.Executors.Routing.RoutesStorage.Models
{
    public class TargetExecutorMethod
    {
        public ExecutorMethod Method { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
