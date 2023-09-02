using Microsoft.Extensions.DependencyInjection;
using Telegramper.Storage.Dictionary;
using Telegramper.Storage.Initializators;
using Telegramper.Storage.List;

namespace Telegramper.Storage.Services
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddListStorage<TItem>(this IServiceCollection services, Func<IServiceProvider, IEnumerable<TItem>> initializeStorage)
        {
            services.AddSingleton<IListStorage<TItem>, ListStorage<TItem>>(
                serviceProvider =>
                {
                    var items = initializeStorage(serviceProvider);
                    return new ListStorage<TItem>(initializeStorage(serviceProvider));
                }
            );
            return services;
        }

        public static IServiceCollection AddDictionaryStorage<TKey, TValue>(this IServiceCollection services, Func<IServiceProvider, IDictionary<TKey, TValue>> initializeStorage)
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

        public static IServiceCollection AddListStorage<TItem, TInitializator>(this IServiceCollection services)
            where TInitializator : class, IListStorageInitializator<TItem>
        {
            services.AddSingleton<TInitializator>();
            services.AddSingleton<IListStorage<TItem>, ListStorage<TItem>>(
                serviceProvider =>
                {
                    var initializator = serviceProvider.GetRequiredService<TInitializator>();
                    var items = initializator.Initialization();
                    return new ListStorage<TItem>(items);
                }
            );
            return services;
        }

        public static IServiceCollection AddDictionaryStorage<TKey, TValue, TInitializator>(this IServiceCollection services)
            where TKey : notnull
            where TInitializator : class, IDictionaryStorageInitializator<TKey, TValue>
        {
            services.AddSingleton<TInitializator>();
            services.AddSingleton<IDictionaryStorage<TKey, TValue>, DictionaryStorage<TKey, TValue>>(
                serviceProvider =>
                {
                    var initializator = serviceProvider.GetRequiredService<TInitializator>();
                    var items = initializator.Initialization();
                    return new DictionaryStorage<TKey, TValue>(items);
                }
            );
            return services;
        }
    }
}
