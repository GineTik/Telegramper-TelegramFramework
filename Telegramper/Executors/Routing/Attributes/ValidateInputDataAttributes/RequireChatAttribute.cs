namespace Telegramper.Executors.Routing.Attributes.ValidateInputDataAttributes
{
    public class RequireChatAttribute : RequireDataAttribute
    {
        public RequireChatAttribute() : base(updateContext => updateContext.Chat)
        {
        }
    }
}
