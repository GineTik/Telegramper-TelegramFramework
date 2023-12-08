using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegramper.Core.Configuration.Services;
using Telegramper.Core.Helpers.Factories.Configuration;

namespace Telegramper.Core
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; } = default!;
        public IConfiguration Configuration { get; } = default!;
        public ReceiverOptions ReceiverOptions { get; } = default!;
        public ILoggingBuilder Logging { get; private set; } = default!;
        public string? ApiKey => _apiKey;

        private string? _apiKey;

        public BotApplicationBuilder()
        {
            Services = new ServiceCollection();
            Configuration = new ConfigurationFactory().CreateConfiguration();
            ReceiverOptions = new ReceiverOptions();

            _apiKey = Configuration["ApiKey"];

            setDefaultsServicesAndLogging();
        }

        public BotApplicationBuilder ConfigureApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public IBotApplication Build()
        {
            if (ApiKey == null)
                throw new NullReferenceException(ApiKey);

            addCurrentBotInformationToServices(ApiKey);
            // if ApiKey is null, will be throw exception in the BotApplication constructor
            return new BotApplication(ApiKey!, Services, ReceiverOptions);
        }

        public static BotApplicationBuilder CreateBuilder() => new BotApplicationBuilder();

        private void addCurrentBotInformationToServices(string apiKey)
        {
            var botClient = new TelegramBotClient(apiKey);
            Services.AddSingleton(new CurrentBot { Data = botClient.GetMeAsync().Result });
        }

        private void setDefaultsServicesAndLogging()
        {
            Services.AddSingleton(Configuration);
            Services.AddUpdateContextAccessor();
            Services.AddLogging(builder => { Logging = builder; });

            Logging.ClearProviders();
            Logging.AddConsole();
        }
    }
}
