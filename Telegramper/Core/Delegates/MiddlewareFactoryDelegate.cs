using Telegramper.Core.Context;

namespace Telegramper.Core.Delegates
{
    internal delegate NextDelegate MiddlewareFactoryDelegate(
        IServiceProvider serviceProvider, 
        UpdateContext updateContext, 
        NextDelegate next);
}
