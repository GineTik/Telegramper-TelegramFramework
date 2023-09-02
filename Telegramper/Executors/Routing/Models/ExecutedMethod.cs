using Telegramper.Executors.Routing.Attributes.BaseAttributes;
using Telegramper.Executors.Routing.ParametersParser.Results;

namespace Telegramper.Executors.Routing.Models
{
    public class ExecutedMethod
    {
        public ExecutorMethod Method { get; set; } = default!;
        public ParametersParseResult? ParametersParseResult { get; set; }
        public ValidationAttribute? FailedValidationAttribute { get; set; }
    }
}
