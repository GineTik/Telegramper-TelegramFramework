using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class RouteTreeMethod
    {
        public ExecutorMethod Method { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }
}
