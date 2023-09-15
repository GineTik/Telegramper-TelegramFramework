using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Telegramper.Core.Helpers.Factories.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Assembly.GetCallingAssembly().Location, "../../../../"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
