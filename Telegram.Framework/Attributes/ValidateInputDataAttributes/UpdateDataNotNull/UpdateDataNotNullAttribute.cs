using Telegram.Framework.Attributes.BaseAttributes;
using Telegram.Framework.TelegramBotApplication.Context;
using Telegram.Bot.Types;

namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class UpdateDataNotNullAttribute : ValidateInputDataAttribute
    {
        protected Func<Update, object?> TakeProperty;

        public UpdateDataNotNullAttribute(Func<Update, object?> takeProperty)
        {
            TakeProperty = takeProperty;
        }

        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            if (TakeProperty == null)
                throw new InvalidOperationException("Func TakeProperty is null");

            return await Task.FromResult(TakeProperty.Invoke(updateContext.Update) != null);
        }
    }
}
