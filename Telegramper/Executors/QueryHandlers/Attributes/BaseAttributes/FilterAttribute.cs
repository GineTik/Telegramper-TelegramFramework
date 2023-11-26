using Telegramper.Core.Context;

namespace Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class FilterAttribute : Attribute
    {
        public virtual Task<bool> BeforeExecutionAsync(IServiceProvider serviceProvider, UpdateContext updateContext)
        { return Task.FromResult(false); }
        public virtual Task AfterExecutionAsync(IServiceProvider serviceProvider, UpdateContext updateContext)
        { return Task.CompletedTask; }
    }
}
