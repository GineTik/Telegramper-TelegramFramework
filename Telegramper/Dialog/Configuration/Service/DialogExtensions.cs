using Microsoft.Extensions.DependencyInjection;
using Telegramper.Dialog.StepFinder;
using Telegramper.Dialog.Instance;
using Telegramper.Dialog.Storages;

namespace Telegramper.Dialog.Configuration.Service
{
    public static class DialogExtensions
    {
        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<IDialogStepFinder, DialogStepFinder>();
            services.AddSingleton<IDialogStepStorage, DialogStepStorage>();
            return services;
        }
    }
}
