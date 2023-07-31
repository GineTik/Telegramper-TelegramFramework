namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireMessageTextAttribute : RequiredDataAttribute
    {
        public RequireMessageTextAttribute() : base(updateContext => updateContext.Message?.Text)
        {
            ErrorMessage = "The text of the message is required";
        }
    }
}
