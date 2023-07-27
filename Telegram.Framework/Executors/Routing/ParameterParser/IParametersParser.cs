using System.Reflection;
using Telegram.Framework.Executors.Routing.ParametersParser.Results;

namespace Telegram.Framework.Executors.Routing.ParametersParser
{
    public interface IParametersParser
    {
        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
