using Telegramper.Core.Context;
using Telegramper.Core.Delegates;

namespace Telegramper.Core.Pipelines
{
    public interface IPipeline
    {
        IPipeline Use(Func<UpdateContext, NextDelegate, Task> middlware);
        IPipeline Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware);
        Task InvokeMiddlewaresAsync(IServiceProvider serviceProvider, UpdateContext updateContext);
    }
}
