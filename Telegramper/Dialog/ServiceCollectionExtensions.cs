using Microsoft.Extensions.DependencyInjection;
using Telegramper.Dialog.Instance;
using Telegramper.Dialog.Models;
using Telegramper.Dialog.StorageInitializers;
using Telegramper.Storage.Services;

namespace Telegramper.Dialog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDialogsByAttributes(this IServiceCollection services)
        {
            services.AddTransient<IDialogService, DialogService>();
            services.AddDictionaryStorage<DialogStepsDictionary, DialogStepsStorageInitializer>();
            return services;
        }
    }
}
