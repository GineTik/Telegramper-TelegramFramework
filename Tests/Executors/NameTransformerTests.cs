using Telegramper.Executors.Initialization.NameTransformer;

namespace Executors;

public class NameTransformerTests
{
    [Theory]
    [InlineData("Start", "start")]
    [InlineData("AddUser", "add_user")]
    [InlineData("AddUser1", "add_user1")]
    public void MethodName_SnakeCase(string methodName, string expected)
    {
        var transformedMethodName = new SnakeCaseNameTransformer().Transform(methodName);
        Assert.Equal(expected, transformedMethodName);
    }
}