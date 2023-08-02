using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.TelegramBotApplication.Delegates
{
    internal delegate NextDelegate MiddlewareFactoryDelegate(
        IServiceProvider serviceProvider, 
        UpdateContext updateContext, 
        NextDelegate next);
}
