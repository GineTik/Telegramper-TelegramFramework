using Microsoft.Extensions.Configuration;

namespace Telegram.Framework.TelegramBotApplication.Helpers.Factories.Configuration
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
