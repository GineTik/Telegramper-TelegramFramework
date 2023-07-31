namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireUserAttribute : RequiredDataAttribute
    {
        public RequireUserAttribute() : base(updateContext => updateContext.User)
        {
        }
    }
}
