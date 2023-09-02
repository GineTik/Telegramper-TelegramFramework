using Telegramper.Executors.Routing.Attributes.BaseAttributes;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Executors.Routing.Attributes.ValidateInputDataAttributes
{
    public class RequireDataAttribute : ValidationAttribute
    {
        protected Func<UpdateContext, object?> TakeProperty;

        public RequireDataAttribute(Func<UpdateContext, object?> takeProperty)
        {
            if (takeProperty == null)
                throw new InvalidOperationException("Func TakeProperty is null");

            TakeProperty = takeProperty;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            return await Task.FromResult(TakeProperty.Invoke(updateContext) != null);
        }
    }
}
