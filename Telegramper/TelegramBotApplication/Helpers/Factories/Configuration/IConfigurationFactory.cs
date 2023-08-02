using Microsoft.Extensions.Configuration;

namespace Telegramper.TelegramBotApplication.Helpers.Factories.Configuration
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
