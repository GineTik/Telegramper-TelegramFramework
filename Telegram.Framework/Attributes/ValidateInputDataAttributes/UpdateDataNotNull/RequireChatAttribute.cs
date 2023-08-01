namespace Telegramper.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireChatAttribute : RequireDataAttribute
    {
        public RequireChatAttribute() : base(updateContext => updateContext.Chat)
        {
        }
    }
}
