using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.TelegramBotApplication.Configuration.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, NextDelegate next);
    }
}
