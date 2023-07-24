using Telegram.Framework.TelegramBotApplication.Configuration.Middlewares;
using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Framework.TelegramBotApplication.Delegates;

namespace Telegram.Framework.TelegramBotApplication
{
    public interface IBotApplication
    {
        IBotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware);
        IBotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware);
        IBotApplication UseMiddleware<T>() where T : class, IMiddleware;
        void PollingRun();
    }
}
