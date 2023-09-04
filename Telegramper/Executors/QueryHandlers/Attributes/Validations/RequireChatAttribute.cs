namespace Telegramper.Executors.QueryHandlers.Attributes.Validations
{
    public class RequireChatAttribute : RequireDataAttribute
    {
        public RequireChatAttribute() : base(updateContext => updateContext.Chat)
        {
        }
    }
}
