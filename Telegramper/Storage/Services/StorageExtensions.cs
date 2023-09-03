using Microsoft.Extensions.DependencyInjection;
using Telegramper.Storage.Dictionary;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Storage.Services
{
    public static class StorageExtensions
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

        public static IServiceCollection AddDictionaryStorage<TKey, TValue>(
            this IServiceCollection services, 
            Func<IServiceProvider, IDictionary<TKey, TValue>> initializeStorage)
            where TKey : notnull
        {
            services.AddSingleton<IDictionaryStorage<TKey, TValue>, DictionaryStorage<TKey, TValue>>(
                serviceProvider =>
                {
                    var items = initializeStorage(serviceProvider);
                    return new DictionaryStorage<TKey, TValue>(items);
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

        public static IServiceCollection AddDictionaryStorage<TKey, TValue, TInitializer>(this IServiceCollection services)
            where TKey : notnull
            where TInitializer : class, IDictionaryStorageInitializer<TKey, TValue>
        {
            services.AddSingleton<TInitializer>();
            services.AddSingleton<IDictionaryStorage<TKey, TValue>, DictionaryStorage<TKey, TValue>>(
                serviceProvider =>
                {
                    var initializator = serviceProvider.GetRequiredService<TInitializer>();
                    var items = initializator.Initialization();
                    return new DictionaryStorage<TKey, TValue>(items);
                }
            );
            return services;
        }
    }
}
