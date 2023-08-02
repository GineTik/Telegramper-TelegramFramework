using Microsoft.Extensions.DependencyInjection;
using Telegramper.Sessions.Implementations;
using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Saver;
using Telegramper.Sessions.Saver.Implementations;

namespace Telegramper.Sessions.Extensions.Services
{
    public static class SessionExtension
    {
        public static IServiceCollection AddSessions(this IServiceCollection services)
        {
            return services.AddSessions<MemorySessionDataSaver>();
        }

        public static IServiceCollection AddSessions<TSaver>(this IServiceCollection services)
            where TSaver : class, ISessionDataSaver
        {
            services.AddTransient<IUserSession, UserSession>();
            services.AddTransient<IChatSession, ChatSession>();
            services.AddSingleton<ISessionDataSaver, TSaver>(); 
            return services;
        }
    }
}
