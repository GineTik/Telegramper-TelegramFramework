using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.Common.Models;

namespace Telegramper.Sequence.Service;

public static class SequenceServiceExtensions
{
    public static async Task StartAsync<TExecutor>(this ISequenceService sequenceService) 
        where TExecutor : Executor
    {
        await sequenceService.StartAsync(typeof(TExecutor));
    }
    
    public static async Task StartAsync(this ISequenceService sequenceService, Type executorType) 
    {
        InvalidTypeException.ThrowIfNotInherit<Executor>(executorType);
        await sequenceService.StartAsync(executorType.FullName!);
    }

    public static async Task NextAsync<TExecutor>(this ISequenceService sequenceService)
        where TExecutor : Executor
    {
        await sequenceService.EndAsync();
        await sequenceService.StartAsync<TExecutor>();
    }
    
    public static async Task NextAsync(this ISequenceService sequenceService, Type executorType)
    {
        await sequenceService.EndAsync();
        await sequenceService.StartAsync(executorType);
    }
    
    public static async Task NextAsync(this ISequenceService sequenceService, int steps = 1)
    {
        if (steps < 1)
        {
            throw new ArgumentOutOfRangeException($"{nameof(steps)}({steps}) cant be less then 1");
        }
        
        await sequenceService.ShiftAsync(steps);
    }

    public static async Task BackAsync(this ISequenceService sequenceService, int steps = 1)
    {
        if (steps < 1)
        {
            throw new ArgumentOutOfRangeException($"{nameof(steps)}({steps}) cant be less then 1");
        }
         
        await sequenceService.ShiftAsync(-steps);
    }
}