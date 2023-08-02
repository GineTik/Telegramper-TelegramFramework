namespace Telegramper.TelegramBotApplication.Exceptions
{
    public class InvalidUpdateTypeException : Exception
    {
        public InvalidUpdateTypeException(string? message) : base(message)
        {
        }
    }
}
