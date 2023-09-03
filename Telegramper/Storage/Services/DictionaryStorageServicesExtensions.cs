using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using Telegramper.Storage.Dictionary;
using Telegramper.Storage.Initializers;

namespace Telegramper.Storage.Services
{
    public static class DictionaryStorageServicesExtensions
    {
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

        public static IServiceCollection AddDictionaryStorage<TDictionary>(
            this IServiceCollection services,
            Func<IServiceProvider, TDictionary> initializeStorage)
            where TDictionary : IDictionary
        {
            services.AddSingleton<IDictionaryStorage<TDictionary>, DictionaryStorage<TDictionary>>(
                serviceProvider =>
                {
                    var items = initializeStorage(serviceProvider);
                    return new DictionaryStorage<TDictionary>(items);
                }
            );
            return services;
        }

        public static IServiceCollection AddDictionaryStorage<TDictionary, TInitializer>(
            this IServiceCollection services)
            where TDictionary : IDictionary
            where TInitializer : class, IDictionaryStorageInitializer<TDictionary>
        {
            services.AddSingleton<TInitializer>();
            services.AddSingleton<IDictionaryStorage<TDictionary>, DictionaryStorage<TDictionary>>(
                serviceProvider =>
                {
                    var initializator = serviceProvider.GetRequiredService<TInitializer>();
                    var items = initializator.Initialization();
                    return new DictionaryStorage<TDictionary>(items);
                }
            );
            return services;
        }
    }
}
