using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegramper.Core.AdvancedBotClient;
using Telegramper.Core.Configuration.Middlewares;
using Telegramper.Core.Configuration.Middlewares.UpdateContexts;
using Telegramper.Core.Context;
using Telegramper.Core.Delegates;
using Telegramper.Core.Pipelines;

namespace Telegramper.Core
{
    public partial class BotApplication : IBotApplication
    {
        public IServiceProvider Services => _publicServiceProvider;
        public ILogger Logger { get; }

        private readonly IPipeline _pipeline;
        private readonly IServiceCollection _serviceCollection;
        private readonly ReceiverOptions _receiverOptions;
        private string _apiKey;
        private IServiceProvider _serviceProviderWithMiddlewares = default!;
        private IServiceProvider _publicServiceProvider = default!;

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
            _serviceCollection = services;
            _receiverOptions = receiverOptions;
            _publicServiceProvider = services.BuildServiceProvider();

            Logger = _publicServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Program");

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
            _serviceCollection.AddTransient<T>();
            _pipeline.Use(async (serviceProvider, updateContext, next) =>
            {
                await serviceProvider.GetRequiredService<T>().InvokeAsync(updateContext, next);
            });
            return this;
        }

        public void RunPolling()
        {
            _serviceProviderWithMiddlewares = _serviceCollection.BuildServiceProvider();

            var client = new TelegramBotClient(_apiKey);
            client.StartReceiving(invokeMiddlewares, handlePollingErrorAsync, _receiverOptions);

            Logger.LogInformation("The bot is up and running");
            Console.ReadLine();
        }

        private async Task invokeMiddlewares(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var updateContext = new UpdateContext();
            updateContext.Update = update;
            updateContext.CancellationToken = cancellationToken;
            updateContext.Client = new AdvancedTelegramBotClient(_apiKey, updateContext);

            await _pipeline.InvokeMiddlewaresAsync(_serviceProviderWithMiddlewares, updateContext);
        }

        private Task handlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Logger.LogError(errorMessage);
            return Task.CompletedTask;
        }
    }
}
