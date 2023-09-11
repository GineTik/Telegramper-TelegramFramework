namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParameterTypeConvertors
{
    public class BasicTypeConvertor : IParameterTypeConvetor
    {
        public object? ConvertTo(Type to, string arg, bool isNullable)
        {
            if (to.IsGenericType && to.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                to = Nullable.GetUnderlyingType(to)!;
            }

            if (to.IsPrimitive == false && to != typeof(Decimal) && to != typeof(String))
            {
                throw new ArgumentException($"Type {to} is not a primitive");
            }

            return Convert.ChangeType(arg, to);
        }
    }
}
