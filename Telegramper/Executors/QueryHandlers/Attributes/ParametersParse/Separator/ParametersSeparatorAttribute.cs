namespace Telegramper.Executors.QueryHandlers.Attributes.ParametersParse.Separator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ParametersSeparatorAttribute : Attribute
    {
        public string Separator { get; }

        public ParametersSeparatorAttribute(string separator)
        {
            ArgumentNullException.ThrowIfNull(separator);
            Separator = separator;
        }
    }
}
