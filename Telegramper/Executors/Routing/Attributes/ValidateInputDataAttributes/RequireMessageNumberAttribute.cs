using Telegramper.Executors.Routing.Attributes.BaseAttributes;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Executors.Routing.Attributes.ValidateInputDataAttributes
{
    public class RequireMessageNumberAttribute : ValidationAttribute
    {
        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            var text = updateContext.Message?.Text;
            return await Task.FromResult(text != null && int.TryParse(text, out _));
        }
    }
}
