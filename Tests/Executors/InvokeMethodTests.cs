using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
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
        
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddUpdateContextAccessor();
        serviceCollection.AddTransient<ISuitableMethodFinder, SuitableMethodFinder>();
        serviceCollection.AddExecutors(options =>
        {
             options.Assemblies = new[] { new SmartAssembly(Assembly.GetExecutingAssembly()) };
        });
        
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    [Theory]
    // this commands exists in the Executors folder
    [InlineData("/start", "Start")]
    public async Task Method_FindSuitableByUpdateContext_SnakeCase(string text, string expectedMethodName)
    {
        var updateContextAccessor = _serviceProvider.GetRequiredService<UpdateContextAccessor>();
        updateContextAccessor.UpdateContext = buildFakeUpdateContext(text);
        
        var suitableMethodFinder = _serviceProvider.GetRequiredService<ISuitableMethodFinder>();
        var suitableMethods = await suitableMethodFinder.FindForCurrentUpdateAsync();

        Assert.Contains(suitableMethods, suitableMethod => suitableMethod.MethodInfo.Name == expectedMethodName);
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