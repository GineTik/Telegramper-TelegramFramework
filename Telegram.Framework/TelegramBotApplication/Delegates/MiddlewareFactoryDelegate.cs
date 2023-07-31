using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.TelegramBotApplication.Delegates
{
    internal delegate NextDelegate MiddlewareFactoryDelegate(
        IServiceProvider serviceProvider, 
        UpdateContext updateContext, 
        NextDelegate next);
}
