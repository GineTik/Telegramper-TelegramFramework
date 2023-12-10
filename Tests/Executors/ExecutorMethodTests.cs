using System.Reflection;
using Executors.Executors;
using Microsoft.Extensions.DependencyInjection;
using Telegramper.Core;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Initialization;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.Initialization.Services;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;
using Telegramper.Executors.QueryHandlers.Attributes.Validations;

namespace Executors;

public class ExecutorMethodTests
{
    private static readonly IEnumerable<FilterAttribute> GlobalAttribute = new[]
    {
        new RequiredDataAttribute(UpdateProperty.User)
    };
    
    private readonly IServiceProvider _serviceProvider;

    public ExecutorMethodTests()
    {
        var builder = BotApplicationBuilder.CreateBuilder();
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
    [InlineData(nameof(ExectorMethodsForTests.Start), new[] { typeof(TargetCommandAttribute) })]
    [InlineData(nameof(ExectorMethodsForTests.CommandWithRequiredDataAttribute), new[] { typeof(TargetCommandAttribute) })]
    public void ExecutorMethod_TargetAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            Array.Empty<FilterAttribute>());
        
        Assert.Equal(expectedAttributes, executorMethod.TargetAttributes.Select(a => a.GetType()));
    }
    
    [Theory]
    [InlineData(nameof(ExectorMethodsForTests.CommandWithRequiredDataAttribute), new[] { typeof(RequiredDataAttribute) })]
    public void ExecutorMethod_FilterAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            Array.Empty<FilterAttribute>());
        
        Assert.Equal(expectedAttributes, executorMethod.FilterAttributes.Select(a => a.GetType()));
    }
    
    [Theory]
    [InlineData(nameof(ExectorMethodsForTests.Start), new Type[0])]
    public void ExecutorMethod_FilterAttributes_WithGlobalAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            GlobalAttribute);
        
        Assert.Equal(
            expectedAttributes.Concat(GlobalAttribute.Select(a => a.GetType())),
            executorMethod.FilterAttributes.Select(a => a.GetType()));
    }
}