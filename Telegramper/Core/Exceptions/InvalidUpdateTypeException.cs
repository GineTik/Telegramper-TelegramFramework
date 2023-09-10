namespace Telegramper.Core.Exceptions
{
    public class InvalidUpdateTypeException : Exception
    {
        public InvalidUpdateTypeException(string? message) : base(message)
        {
        }
    }
}
