using System.Text.RegularExpressions;

namespace Telegramper.Executors.NameTransformer
{
    public class SnakeCaseNameTransformer : INameTransformer
    {
        public string Transform(string methodName)
        {
            ArgumentNullException.ThrowIfNull(methodName);

            methodName = Regex.Replace(methodName, "(.)([A-Z][a-z]+)", "$1_$2");
            methodName = Regex.Replace(methodName, "([a-z0-9])([A-Z])", "$1_$2");
            return methodName.ToLower();
        }
    }
}
