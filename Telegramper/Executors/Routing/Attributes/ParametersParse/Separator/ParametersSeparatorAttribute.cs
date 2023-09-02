namespace Telegramper.Executors.Routing.Attributes.ParametersParse.Separator
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
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
