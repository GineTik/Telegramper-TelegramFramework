using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Core.Context;

namespace Telegramper.Executors.QueryHandlers.Attributes.Validations
{
    public enum UpdateProperty
    {
        User,
        Chat,
        MessageText,
        MessagePhoto,
        CallbackData
    }
    
    public class RequiredDataAttribute : ValidationAttribute
    {
        private readonly Func<UpdateContext, object?> _propertyByUpdateContext;
        
        public RequiredDataAttribute(UpdateProperty updateProperty)
        {
            _propertyByUpdateContext = updateProperty switch
            {
                UpdateProperty.User => (updateContext) => updateContext.User,
                UpdateProperty.Chat => (updateContext) => updateContext.Chat,
                UpdateProperty.MessageText => (updateContext) => updateContext.Message?.Text,
                UpdateProperty.MessagePhoto => (updateContext) => updateContext.Message?.Photo,
                UpdateProperty.CallbackData => (updateContext) => updateContext.Update.CallbackQuery?.Data,
                _ => throw new NotSupportedException()
            };
        }
        
        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            return await Task.FromResult(_propertyByUpdateContext(updateContext) != null);
        }
    }
}
