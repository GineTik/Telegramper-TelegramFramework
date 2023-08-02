namespace Telegramper.Executors.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequireMessagePhotoAttribute : RequireDataAttribute
    {
        public RequireMessagePhotoAttribute() : base(updateContext => updateContext.Message?.Photo)
        {
            ErrorMessage = "The photo of the message is required";
        }
    }
}
