using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegramper.TelegramBotApplication.AdvancedBotClient;
using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Configuration.Middlewares.UpdateContexts;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.TelegramBotApplication
{
    public partial class BotApplication : IBotApplication
    {
        private readonly Stack<MiddlewareFactoryDelegate> _middlewareFactoies;
        private readonly IServiceCollection _services;
        private readonly ReceiverOptions _receiverOptions;
        private string _apiKey;
        private IServiceProvider _globalServiceProvider = default!;

        public BotApplication(
            string apiKey, 
            IServiceCollection services, 
            ReceiverOptions receiverOptions)
        {
            ArgumentNullException.ThrowIfNull(apiKey);
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(receiverOptions);

            _middlewareFactoies = new();
            _apiKey = apiKey;
            _services = services;
            _receiverOptions = receiverOptions;

            UseMiddleware<UpdateContextMiddleware>();
        }

        public IBotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewareFactoies.Push((serviceProvider, updateContext, next) =>
                async () => await middlware(updateContext, next));

            return this;
        }

        public IBotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)
        {
            ArgumentNullException.ThrowIfNull(middlware);

            _middlewareFactoies.Push((serviceProvider, updateContext, next) =>
               async () => await middlware(serviceProvider, updateContext, next));

            return this;
        }

         public IBotApplication UseMiddleware<T>()
            where T : class, IMiddleware
        {
            _services.AddTransient<T>();
            _middlewareFactoies.Push((serviceProvider, updateContext, next) =>
                async () => await serviceProvider.GetRequiredService<T>().InvokeAsync(updateContext, next));

            return this;
        }

        public void RunPolling()
        {
            _globalServiceProvider = _services.BuildServiceProvider();

            var client = new TelegramBotClient(_apiKey);
            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync, _receiverOptions);

            Console.ReadLine();
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var updateContext = new UpdateContext();
            updateContext.Update = update;
            updateContext.CancellationToken = cancellationToken;
            updateContext.Client = new AdvancedTelegramBotClient(_apiKey, updateContext);

            using (var scope = _globalServiceProvider.CreateScope())
            {
                var firstMiddleware = buildMiddlewares(scope, updateContext);
                await firstMiddleware.Invoke();
            }
        }

        private Task handlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
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
