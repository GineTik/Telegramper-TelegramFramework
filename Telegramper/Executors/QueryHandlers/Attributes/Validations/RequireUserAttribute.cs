namespace Telegramper.Executors.QueryHandlers.Attributes.Validations
{
    public class RequireUserAttribute : RequireDataAttribute
    {
        public RequireUserAttribute() : base(updateContext => updateContext.User)
        {
        }
    }
}
