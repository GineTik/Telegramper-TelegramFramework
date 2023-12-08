namespace Telegramper.Executors.Common.Options;

public class HandlerQueueOptions
{
    /// default values see in <see cref="ExecutorOptions"/>
    
    public const int InfinityHandlers = -1;
    
    public required int LimitOfHandlersPerRequest { get; set; }
}