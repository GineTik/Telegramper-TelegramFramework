namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class UpdatePhotoNotNullAttribute : UpdateDataNotNullAttribute
    {
        public UpdatePhotoNotNullAttribute() : base(update => update.Message?.Photo)
        {
            ErrorMessage = "Photo is null";
        }
    }
}
