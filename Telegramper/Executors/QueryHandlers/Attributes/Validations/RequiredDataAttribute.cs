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
    }
    
    public class RequiredDataAttribute : ValidationAttribute
    {
        private readonly UpdateProperty _updateProperty;
        
        public RequiredDataAttribute(UpdateProperty updateProperty)
        {
            _updateProperty = updateProperty;
        }
        
        public override async Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider)
        {
            object? propertyValue = _updateProperty switch
            {
                UpdateProperty.User => updateContext.User,
                UpdateProperty.Chat => updateContext.Chat,
                UpdateProperty.MessageText => updateContext.Message?.Text,
                UpdateProperty.MessagePhoto => updateContext.Message?.Photo,
                _ => throw new NotImplementedException()
            };
            
            return await Task.FromResult(propertyValue != null);
        }
    }
}
