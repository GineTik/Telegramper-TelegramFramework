namespace Telegram.Framework.Attributes.ValidateInputDataAttributes.UpdateDataNotNull
{
    public class RequiredMessagePhotoAttribute : RequiredDataAttribute
    {
        public RequiredMessagePhotoAttribute() : base(updateContext => updateContext.Message?.Photo)
        {
            ErrorMessage = "The photo of the message is required";
        }
    }
}
