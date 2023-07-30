namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredUserAttribute : RequiredDataAttribute
    {
        public RequiredUserAttribute() : base(updateContext => updateContext.User)
        {
        }
    }
}
