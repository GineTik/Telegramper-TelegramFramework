using System.Reflection;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.ParameterParser.Models;

namespace Telegramper.Executors.QueryHandlers.ParameterParser
{
    public interface IParametersParser
    {
        public ParametersParseResult TryParse(ExecutorMethod method);
    }
}
