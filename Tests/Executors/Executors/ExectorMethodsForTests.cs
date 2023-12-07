using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;

namespace Executors.Executors;

public class ExectorMethodsForTests : Executor
{
    [TargetCommand]
    public Task Start()
    {
        return Task.CompletedTask;
    }
}