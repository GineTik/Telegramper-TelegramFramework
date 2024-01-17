using Microsoft.Extensions.DependencyInjection;
using Telegramper.Sequence.Models;
using Telegramper.Sequence.Service;
using Telegramper.Sequence.StorageInitializers;
using Telegramper.Storage.Services;

namespace Telegramper.Sequence
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
