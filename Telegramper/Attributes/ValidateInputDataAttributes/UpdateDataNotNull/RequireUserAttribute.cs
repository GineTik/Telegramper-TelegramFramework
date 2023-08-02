namespace Telegramper.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireUserAttribute : RequireDataAttribute
    {
        public RequireUserAttribute() : base(updateContext => updateContext.User)
        {
        }
    }
}
