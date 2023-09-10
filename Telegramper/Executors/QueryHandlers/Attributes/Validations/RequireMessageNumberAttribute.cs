using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Core.Context;

namespace Telegramper.Executors.QueryHandlers.Attributes.Validations
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
