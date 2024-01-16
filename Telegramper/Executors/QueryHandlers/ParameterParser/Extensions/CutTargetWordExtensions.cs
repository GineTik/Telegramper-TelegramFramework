using System.Text.RegularExpressions;

namespace Telegramper.Executors.QueryHandlers.ParameterParser.Extensions;

public static class CutTargetWordExtensions
{
    public static string RemoveCommand(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        return Regex.Replace(text, "^/\\w+\\s*", "");
    }
}