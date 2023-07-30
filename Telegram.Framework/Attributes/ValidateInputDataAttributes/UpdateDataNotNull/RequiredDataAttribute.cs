using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredDataAttribute : ValidateInputDataAttribute
    {
        protected Func<UpdateContext, object?> TakeProperty;

        public RequiredDataAttribute(Func<UpdateContext, object?> takeProperty)
        {
            TakeProperty = takeProperty;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            if (TakeProperty == null)
                throw new InvalidOperationException("Func TakeProperty is null");

            return await Task.FromResult(TakeProperty.Invoke(updateContext) != null);
        }
    }
}
