using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class Route
    {
        public required ExecutorMethod Method { get; set; }
        public required TargetAttribute TargetAttribute { get; set; }
    }
}
