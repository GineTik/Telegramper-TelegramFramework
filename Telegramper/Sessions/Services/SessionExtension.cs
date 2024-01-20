using Microsoft.Extensions.DependencyInjection;
using Telegramper.Sessions.Implementations;
using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Options;
using Telegramper.Sessions.Saver;

namespace Telegramper.Sessions.Services
{
    public static class SessionExtension
    {
        public static IServiceCollection AddSessions(this IServiceCollection services, SessionOptions options)
        {
            services.AddTransient<IUserSession, UserSession>();
            services.AddTransient<IChatSession, ChatSession>();
            services.AddSingleton(typeof(ISessionSaveStrategy), options.SaveStrategyType); 
            return services;
        }
    }
}
