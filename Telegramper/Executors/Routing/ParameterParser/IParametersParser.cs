using System.Reflection;
using Telegramper.Executors.Routing.ParametersParser.Results;

namespace Telegramper.Executors.Routing.ParametersParser
{
    public interface IParametersParser
    {
        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
