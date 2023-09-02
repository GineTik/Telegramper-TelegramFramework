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
using Telegramper.TelegramBotApplication.Pipelines;

namespace Telegramper.TelegramBotApplication
{
    public partial class BotApplication : IBotApplication
    {
        private readonly IPipeline _pipeline;
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

            _pipeline = new Pipeline();
            _apiKey = apiKey;
            _services = services;
            _receiverOptions = receiverOptions;

            UseMiddleware<UpdateContextMiddleware>();
        }

        public IBotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware)
        {
            _pipeline.Use(middlware);
            return this;
        }

        public IBotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)
        {
            _pipeline.Use(middlware);
            return this;
        }

         public IBotApplication UseMiddleware<T>()
            where T : class, IMiddleware
        {
            _services.AddTransient<T>();
            _pipeline.Use(async (serviceProvider, updateContext, next) =>
            {
                await serviceProvider.GetRequiredService<T>().InvokeAsync(updateContext, next);
            });
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

            await _pipeline.InvokeMiddlewaresAsync(_globalServiceProvider, updateContext);
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
    }
}
