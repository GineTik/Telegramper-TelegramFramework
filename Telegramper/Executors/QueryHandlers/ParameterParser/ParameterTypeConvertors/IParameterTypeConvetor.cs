namespace Telegramper.Executors.QueryHandlers.ParameterParser.ParameterTypeConvertors
{
    public interface IParameterTypeConvetor
    {
        object? ConvertTo(Type to, string parameter, bool isNullable);
    }
}
