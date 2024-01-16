using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;
using Telegramper.Executors.QueryHandlers.Attributes.Validations;

namespace Executors.Executors;

public class ExectorMethodsForTests : Executor
{
    [TargetCommand]
    public Task Start()
    {
        return Task.CompletedTask;
    }
    
    [TargetCommand]
    [RequiredData(UpdateProperty.User)]
    public Task CommandWithRequiredDataAttribute()
    {
        return Task.CompletedTask;
    }
}