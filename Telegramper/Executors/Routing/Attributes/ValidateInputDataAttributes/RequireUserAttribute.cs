namespace Telegramper.Executors.Routing.Attributes.ValidateInputDataAttributes
{
    public class RequireUserAttribute : RequireDataAttribute
    {
        public RequireUserAttribute() : base(updateContext => updateContext.User)
        {
        }
    }
}
