using Microsoft.Extensions.DependencyInjection;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Storage.Services
{
    public static class ListStorageServicesExtensions
    {
        public static IServiceCollection AddListStorage<TItem>(
            this IServiceCollection services,
            Func<IServiceProvider, IEnumerable<TItem>> initializeStorage)
        {
            services.AddSingleton<IListStorage<TItem>, ListStorage<TItem>>(
                serviceProvider =>
                {
                    var items = initializeStorage(serviceProvider).ToList();
                    return new ListStorage<TItem>(items);
                }
            );
            return services;
        }

        public static IServiceCollection AddListStorage<TItem, TInitializer>(this IServiceCollection services)
            where TInitializer : class, IListStorageInitializer<TItem>
        {
            services.AddSingleton<TInitializer>();
            services.AddSingleton<IListStorage<TItem>, ListStorage<TItem>>(
                serviceProvider =>
                {
                    var initializator = serviceProvider.GetRequiredService<TInitializer>();
                    var items = initializator.Initialization().ToList();
                    return new ListStorage<TItem>(items);
                }
            );
            return services;
        }

    }
}
