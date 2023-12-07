using Executors.Executors;
using Microsoft.Extensions.DependencyInjection;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;
using Telegramper.Executors.QueryHandlers.Attributes.Validations;

namespace Executors;

public class ExecutorMethodTests
{
    private static readonly IEnumerable<FilterAttribute> _globalAttrbiute = new[]
    {
        new RequiredDataAttribute(UpdateProperty.User)
    };
    
    private readonly IServiceProvider _serviceProvider;

    public ExecutorMethodTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<INameTransformer, SnakeCaseNameTransformer>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    [Theory]
    [InlineData(nameof(ExectorMethodsForTests.Start), new[] { typeof(TargetCommandAttribute) })]
    [InlineData(nameof(ExectorMethodsForTests.CommandWithRequiredDataAttribute), new[] { typeof(TargetCommandAttribute) })]
    public void ExecutorMethod_TargetAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            Array.Empty<Attribute>());
        
        Assert.Equal(expectedAttributes, executorMethod.TargetAttributes.Select(a => a.GetType()));
    }
    
    [Theory]
    [InlineData(nameof(ExectorMethodsForTests.CommandWithRequiredDataAttribute), new[] { typeof(RequiredDataAttribute) })]
    public void ExecutorMethod_FilterAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            Array.Empty<Attribute>());
        
        Assert.Equal(expectedAttributes, executorMethod.FilterAttributes.Select(a => a.GetType()));
    }
    
    [Theory]
    [InlineData(nameof(ExectorMethodsForTests.Start), new Type[0])]
    public void ExecutorMethod_FilterAttributes_WithGlobalAttributes(string methodName, IEnumerable<Type> expectedAttributes)
    {
        var executorMethod = new ExecutorMethod(
            typeof(ExectorMethodsForTests).GetMethod(methodName)!, 
            _serviceProvider, 
            _globalAttrbiute);
        
        Assert.Equal(
            expectedAttributes.Concat(_globalAttrbiute.Select(a => a.GetType())),
            executorMethod.FilterAttributes.Select(a => a.GetType()));
    }
}