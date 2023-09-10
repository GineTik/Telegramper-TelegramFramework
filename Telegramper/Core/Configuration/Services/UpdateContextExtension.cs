using Telegramper.Core.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Telegramper.Core.Configuration.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
