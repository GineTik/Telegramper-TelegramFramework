using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.ParametersParser.Results;

namespace Telegramper.Executors.QueryHandlers.Preparer.ErrorHandler
{
    public interface IParseErrorHandler
    {
        string GetErrorMessage(ParseStatus status, ExecutorMethod method);
    }
}
