using Microsoft.Extensions.DependencyInjection;
using Telegramper.Core.Pipelines;

namespace Core;

public class PipelineTests
{
    private static readonly IServiceProvider FakeServiceProvider = new ServiceCollection().BuildServiceProvider();
    
    [Fact]
    public async Task Middleware_TwoParameters_NotCalled()
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
    public async Task Middleware_TwoParameters_Called()
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
    
    [Fact]
    public async Task Middleware_ThreeParameters_NotCalled()
    {
        var middlewareInvoked = false;

        var pipeline = new Pipeline();
        pipeline.Use((_, _, _) => Task.CompletedTask);
        pipeline.Use(async (_, _, next) =>
        {
            middlewareInvoked = true;
            await next();
        });
        await pipeline.InvokeMiddlewaresAsync(FakeServiceProvider, null!);
        
        Assert.False(middlewareInvoked);
    }
    
    [Fact]
    public async Task Middleware_ThreeParameters_Called()
    {
        var middlewareInvoked = false;

        var pipeline = new Pipeline();
        pipeline.Use(async (_, _, next) => await next());
        pipeline.Use(async (_, _, next) =>
        {
            middlewareInvoked = true;
            await next();
        });
        await pipeline.InvokeMiddlewaresAsync(FakeServiceProvider, null!);
        
        Assert.True(middlewareInvoked);
    }
}