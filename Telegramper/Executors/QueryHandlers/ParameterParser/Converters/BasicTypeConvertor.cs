namespace Telegramper.Executors.QueryHandlers.ParameterParser.Converters
{
    public class BasicTypeConvertor : IParameterTypeConverter
    {
        public bool CanConvert(Type to)
        {
            return to.IsPrimitive || to == typeof(decimal) || to == typeof(string);
        }

        public object? ConvertTo(Type to, string arg, bool isNullable)
        {
            if (to.IsGenericType && to.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                to = Nullable.GetUnderlyingType(to)!;
            }

            return Convert.ChangeType(arg, to);
        }
    }
}
