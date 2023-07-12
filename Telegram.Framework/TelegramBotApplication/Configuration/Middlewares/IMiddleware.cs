using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Framework.TelegramBotApplication.Delegates;

namespace Telegram.Framework.TelegramBotApplication.Configuration.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, NextDelegate next);
    }
}
