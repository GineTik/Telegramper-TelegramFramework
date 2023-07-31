namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireChatAttribute : RequiredDataAttribute
    {
        public RequireChatAttribute() : base(updateContext => updateContext.Chat)
        {
        }
    }
}
