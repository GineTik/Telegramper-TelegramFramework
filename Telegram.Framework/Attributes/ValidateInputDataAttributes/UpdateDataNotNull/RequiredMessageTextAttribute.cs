namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredMessageTextAttribute : UpdateDataNotNullAttribute
    {
        public RequiredMessageTextAttribute() : base(update => update.Message?.Text)
        {
            ErrorMessage = "The text of the message is required";
        }
    }
}
