namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredMessageTextAttribute : RequiredDataAttribute
    {
        public RequiredMessageTextAttribute() : base(updateContext => updateContext.Message?.Text)
        {
            ErrorMessage = "The text of the message is required";
        }
    }
}
