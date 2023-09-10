using Telegramper.Core.Context;
using Telegramper.Core.Delegates;

namespace Telegramper.Core.Configuration.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, NextDelegate next);
    }
}
