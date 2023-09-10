using Microsoft.Extensions.Configuration;

namespace Telegramper.Core.Helpers.Factories.Configuration
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
