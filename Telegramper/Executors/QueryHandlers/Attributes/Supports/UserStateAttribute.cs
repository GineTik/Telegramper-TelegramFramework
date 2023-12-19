namespace Telegramper.Executors.QueryHandlers.Attributes.Supports;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UserStateAttribute : Attribute
{
    public IEnumerable<string> UserStates { get; }

    public UserStateAttribute(params string[] userStates)
    {
        UserStates = userStates;
    }
}