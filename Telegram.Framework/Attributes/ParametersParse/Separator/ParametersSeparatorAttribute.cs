namespace Telegram.Framework.Attributes.ParametersParse.Separator
{
    [AttributeUsage(AttributeTargets.Method)]
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
