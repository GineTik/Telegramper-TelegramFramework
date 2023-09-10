using Telegramper.Core.Context;
using Telegramper.Core.Delegates;

namespace Telegramper.Core.Configuration.Middlewares.UpdateContexts
{
    public class UpdateContextMiddleware : IMiddleware
    {
        private UpdateContextAccessor _accessor;

        public UpdateContextMiddleware(UpdateContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            _accessor.UpdateContext = updateContext;
            await next();
        }
    }
}
