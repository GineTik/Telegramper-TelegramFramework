using System.Text.RegularExpressions;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.Extensions
{
    public static class RemoveFirstTargetWordExetensions
    {
        public static string RemoveCommand(this string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            return Regex.Replace(text, "^/\\w+\\s*", "");
        }

        public static string RemoveFirstTargetCallbackData(this string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            if (text.Length == 0)
            {
                throw new ArgumentOutOfRangeException("Length of text is 0");
            }

            int index = text.IndexOf(" ") + 1;
            return text.Substring(index);
        }
    }
}
