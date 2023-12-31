using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.ParameterParser.Models;

namespace Telegramper.Executors.QueryHandlers.Models
{
    public class ExecutedMethod
    {
        public ExecutorMethod Method { get; set; } = default!;
        public ParametersParseResult? ParametersParseResult { get; set; }
        public ValidationAttribute? FailedValidationAttribute { get; set; }
    }
}
