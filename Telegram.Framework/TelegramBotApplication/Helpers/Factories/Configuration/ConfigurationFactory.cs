using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Telegram.Framework.TelegramBotApplication.Helpers.Factories.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Assembly.GetCallingAssembly().Location, "../../../../"))
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
