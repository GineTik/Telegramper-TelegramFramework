using System.Reflection;
using Telegramper.Attributes.BaseAttributes;
using Telegramper.Executors.Routing.ParametersParser.Results;

namespace Telegramper.Executors.Routing.Models
{
    public class ExecutedMethodMetadata
    {
        public Type ExecutorType => 
            MethodInfo.DeclaringType ??
            MethodInfo.ReflectedType ??
            throw new InvalidOperationException($"Method {MethodInfo.Name} don't have DeclaringType and ReflectedType");

        public MethodInfo MethodInfo { get; set; } = default!;

        public ParametersParseResult? ParametersParseResult { get; set; }

        public ValidateInputDataAttribute? FailedValidationAttribute { get; set; }
    }
}
