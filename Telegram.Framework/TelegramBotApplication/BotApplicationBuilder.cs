using Telegram.Framework.TelegramBotApplication.Configuration.Services;
using Telegram.Framework.TelegramBotApplication.Helpers.Factories.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;

namespace Telegram.Framework.TelegramBotApplication
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public ReceiverOptions ReceiverOptions { get; }

        private string? _apiKey;
        public string? ApiKey => _apiKey;

        public BotApplicationBuilder()
        {
            Services = new ServiceCollection();
            Configuration = new ConfigurationFactory().CreateConfiguration();
            ReceiverOptions = new ReceiverOptions();
            _apiKey = Configuration["ApiKey"];

            Services.AddSingleton(Configuration);
            Services.AddUpdateContextAccessor();
        }

        public BotApplicationBuilder ConfigureApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public IBotApplication Build()
        {
            // if ApiKey is null, will be throw exception in the constructor
            return new BotApplication(ApiKey!, Services, ReceiverOptions);
        }

        public static BotApplicationBuilder CreateBuilder() => new BotApplicationBuilder();
    }
}
