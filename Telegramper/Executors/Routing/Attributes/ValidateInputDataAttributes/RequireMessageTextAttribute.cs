namespace Telegramper.Executors.Routing.Attributes.ValidateInputDataAttributes
{
    public class RequireMessageTextAttribute : RequireDataAttribute
    {
        public RequireMessageTextAttribute() : base(updateContext => updateContext.Message?.Text)
        {
            ErrorMessage = "The text of the message is required";
        }
    }
}
