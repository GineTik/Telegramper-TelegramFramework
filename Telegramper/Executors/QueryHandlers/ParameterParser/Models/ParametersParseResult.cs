namespace Telegramper.Executors.QueryHandlers.ParameterParser.Models
{
    public class ParametersParseResult
    {
        public object?[] ConvertedParameters { get; set; } = default!;
        public string? ErrorMessage { get; set; }

        public bool Successfully => ErrorMessage == null;

        public static ParametersParseResult Success(object?[] convertedParameters) => new ParametersParseResult
        {
            ConvertedParameters = convertedParameters,
            ErrorMessage = null
        };
    }
}
