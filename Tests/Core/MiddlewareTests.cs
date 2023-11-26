using Microsoft.Extensions.DependencyInjection;
using Telegramper.Core.Pipelines;

namespace Core;

public class MiddlewareTests
{
    private static readonly IServiceProvider FakeServiceProvider = new ServiceCollection().BuildServiceProvider();
    
    [Fact]
    public async Task Middleware_NotInvoking()
    {
        var middlewareInvoked = false;
        
        var pipeline = new Pipeline();
        pipeline.Use((_, _) => Task.CompletedTask);
        pipeline.Use(async (_, next) =>
        {
            middlewareInvoked = true;
            await next();
        });
        await pipeline.InvokeMiddlewaresAsync(FakeServiceProvider, null!);
        
        Assert.False(middlewareInvoked);
    }
    
    [Fact]
    public async Task Middleware_Invoking()
    {
        var middlewareInvoked = false;

        var pipeline = new Pipeline();
        pipeline.Use(async (_, next) => await next());
        pipeline.Use(async (_, next) =>
        {
            middlewareInvoked = true;
            await next();
        });
        await pipeline.InvokeMiddlewaresAsync(FakeServiceProvider, null!);
        
        Assert.True(middlewareInvoked);
    }
}