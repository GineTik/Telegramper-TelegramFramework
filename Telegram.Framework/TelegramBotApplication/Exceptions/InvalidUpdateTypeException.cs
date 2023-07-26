namespace Telegram.Framework.TelegramBotApplication.Exceptions
{
    public class InvalidUpdateTypeException : Exception
    {
        public InvalidUpdateTypeException(string? message) : base(message)
        {
        }
    }
}
