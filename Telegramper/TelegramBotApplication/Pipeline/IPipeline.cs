using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.TelegramBotApplication.Pipelines
{
    public interface IPipeline
    {
        IPipeline Use(Func<UpdateContext, NextDelegate, Task> middlware);
        IPipeline Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware);
        Task InvokeMiddlewaresAsync(IServiceProvider serviceProvider, UpdateContext updateContext);
    }
}
