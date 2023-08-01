using Telegramper.Session.Storage;
using Telegramper.Session.Storage.Saver;
using Telegramper.Session.Storage.Saver.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Telegramper.Session.Extensions.Services
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
            services.AddTransient(typeof(ISession<>), typeof(Session<>));
            services.AddTransient<ISessionDataStorage, SessionDataStorage>();
            services.AddSingleton<ISessionDataSaver, TSaver>();
            return services;
        }
    }
}
