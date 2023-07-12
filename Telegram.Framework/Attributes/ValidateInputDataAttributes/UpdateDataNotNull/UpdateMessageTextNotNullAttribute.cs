namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class UpdateMessageTextNotNullAttribute : UpdateDataNotNullAttribute
    {
        public UpdateMessageTextNotNullAttribute() : base(update => update.Message?.Text)
        {
            ErrorMessage = "Text is null";
        }
    }
}
