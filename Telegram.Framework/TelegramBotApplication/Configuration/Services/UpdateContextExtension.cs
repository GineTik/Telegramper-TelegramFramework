using Telegramper.TelegramBotApplication.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Telegramper.TelegramBotApplication.Configuration.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
