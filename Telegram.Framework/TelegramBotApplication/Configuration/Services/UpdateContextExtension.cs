using Telegram.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Framework.TelegramBotApplication.Configuration.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
