namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredMessagePhotoAttribute : UpdateDataNotNullAttribute
    {
        public RequiredMessagePhotoAttribute() : base(update => update.Message?.Photo)
        {
            ErrorMessage = "The photo of the message is required";
        }
    }
}
