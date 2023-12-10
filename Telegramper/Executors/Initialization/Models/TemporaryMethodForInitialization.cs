using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Initialization.Models;

public class TemporaryMethodDataForInitialization
{
    public required TargetAttribute TargetAttribute { get; set; }
    public required ExecutorMethod Method { get; set; }
}