using System.Reflection;

namespace Telegram.Framework.Executors.Helpers.Extensions.Nullable
{
    public static class ParameterInfoExtensions
    {
        public static bool IsNullable(this ParameterInfo property)
        {
            ArgumentNullException.ThrowIfNull(property);
            var nullableInfoContext = new NullabilityInfoContext();
            var result = nullableInfoContext.Create(property).WriteState == NullabilityState.Nullable;
            return result;
        }

        public static int NullableCount(this IEnumerable<ParameterInfo> parameters)
        {
            return parameters.Count(p => p.IsNullable());
        }
    }
}
