using Microsoft.Extensions.DependencyInjection;
using Telegramper.Dialog.Service;
using Telegramper.Dialog.Models;
using Telegramper.Dialog.StorageInitializers;
using Telegramper.Storage.Services;

namespace Telegramper.Dialog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSequence(this IServiceCollection services)
        {
            services.AddTransient<ISequenceService, SequenceService>();
            services.AddDictionaryStorage<SequenceDictionary, SequenceStorageInitializer>();
            return services;
        }
    }
}
