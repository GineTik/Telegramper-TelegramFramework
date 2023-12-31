using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegramper.Core;
using Telegramper.Core.Configuration.Services;
using Telegramper.Core.Context;
using Telegramper.Executors.Initialization;
using Telegramper.Executors.Initialization.Services;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder;
using Xunit.Abstractions;

namespace Executors;

public class InvokeMethodTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IServiceProvider _serviceProvider;

    public InvokeMethodTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var builder = new BotApplicationBuilder();
        builder.Services.AddExecutors(options =>
        {
            options.Assemblies = new[]
            {
                new SmartAssembly(Assembly.GetExecutingAssembly())
            };
        });
        _serviceProvider = builder.Build().Services;
    }
    
    [Theory]
    // this commands exists in the Executors folder
    [InlineData("/start", "Start")]
    public async Task Method_FindSuitableByUpdateContext_SnakeCase(string text, string expectedMethodName)
    {
        var updateContextAccessor = _serviceProvider.GetRequiredService<UpdateContextAccessor>();
        updateContextAccessor.UpdateContext = buildFakeUpdateContext(text);
        var suitableMethodFinder = _serviceProvider.GetRequiredService<ISuitableMethodFinder>();
        
        var suitableRoutes = await suitableMethodFinder.FindForCurrentUpdateAsync();

        Assert.Contains(suitableRoutes, suitableRoute => suitableRoute.Method.MethodInfo.Name == expectedMethodName);
    }
    
    private static UpdateContext buildFakeUpdateContext(string text)
    {
        return new UpdateContext
        {
            Update = new Update
            {
                Message = new Message
                {
                    From = new User
                    {
                        Id = 12345
                    },
                    Text = text,
                }
            },
            CancellationToken = new CancellationToken()
        };
    }
}