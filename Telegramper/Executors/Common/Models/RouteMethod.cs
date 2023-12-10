using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class RouteMethod
    {
        public required ExecutorMethod Method { get; set; }
        public required ICollection<TargetAttribute> TargetAttributes { get; set; }
    }
}
