using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.TelegramBotApplication
{
    public interface IBotApplication
    {
        IBotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware);
        IBotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware);
        IBotApplication UseMiddleware<T>() where T : class, IMiddleware;
        void RunPolling();
    }
}
