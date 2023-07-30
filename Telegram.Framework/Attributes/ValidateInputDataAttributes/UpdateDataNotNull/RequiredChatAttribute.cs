namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredChatAttribute : RequiredDataAttribute
    {
        public RequiredChatAttribute() : base(updateContext => updateContext.Chat)
        {
        }
    }
}
