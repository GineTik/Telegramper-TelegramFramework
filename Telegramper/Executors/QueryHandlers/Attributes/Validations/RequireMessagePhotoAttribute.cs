namespace Telegramper.Executors.QueryHandlers.Attributes.Validations
{
    public class RequireMessagePhotoAttribute : RequireDataAttribute
    {
        public RequireMessagePhotoAttribute() : base(updateContext => updateContext.Message?.Photo)
        {
            ErrorMessage = "The photo of the message is required";
        }
    }
}
