using Microsoft.Extensions.DependencyInjection;
using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.TelegramBotApplication.Pipelines
{
    public class Pipeline : IPipeline
    {
        private readonly Stack<MiddlewareFactoryDelegate> _middlewareFactoies;

        public Pipeline()
        {
            _middlewareFactoies = new();
        }

        public IPipeline Use(Func<UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewareFactoies.Push((serviceProvider, updateContext, next) =>
                async () => await middlware(updateContext, next));

            return this;
        }

        public IPipeline Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewareFactoies.Push((serviceProvider, updateContext, next) =>
               async () => await middlware(serviceProvider, updateContext, next));

            return this;
        }

        public async Task InvokeMiddlewaresAsync(IServiceProvider globalServiceProvider, UpdateContext updateContext)
        {
            using (var scope = globalServiceProvider.CreateScope())
            {
                var firstMiddleware = buildMiddlewares(scope, updateContext);
                await firstMiddleware.Invoke();
            }
        }

        private NextDelegate buildMiddlewares(IServiceScope scope, UpdateContext updateContext)
        {
            NextDelegate firstMiddleware = () => Task.CompletedTask;

            foreach (var factory in _middlewareFactoies)
            {
                firstMiddleware = factory.Invoke(scope.ServiceProvider, updateContext, firstMiddleware);
            }

            return firstMiddleware;
        }
    }
}
