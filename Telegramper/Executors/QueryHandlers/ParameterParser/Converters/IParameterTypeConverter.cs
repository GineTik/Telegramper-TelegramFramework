namespace Telegramper.Executors.QueryHandlers.ParameterParser.Converters
{
    public interface IParameterTypeConverter
    {
        bool CanConvert(Type to);
        object? ConvertTo(Type to, string parameter, bool isNullable);
    }
}
