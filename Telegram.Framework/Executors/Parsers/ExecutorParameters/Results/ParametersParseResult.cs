namespace Telegram.Framework.Executors.Parsers.ExecutorParameters.Results
{
    public enum ParseStatus
    {
        Success,
        ParseError,
        ArgsLengthIsLess
    }

    public class ParametersParseResult
    {
        public object?[] ConvertedParameters { get; set; } = default!;
        public ParseStatus Status { get; set; }

        public static ParametersParseResult Success(object?[] convertedParameters) => new ParametersParseResult
        {
            ConvertedParameters = convertedParameters,
            Status = ParseStatus.Success
        };

        public static ParametersParseResult ArgsLengthIsLess => new ParametersParseResult
        {
            ConvertedParameters = new object?[0],
            Status = ParseStatus.ArgsLengthIsLess
        };

        public static ParametersParseResult ParseError => new ParametersParseResult
        {
            ConvertedParameters = new object?[0],
            Status = ParseStatus.ParseError
        };
    }
}
