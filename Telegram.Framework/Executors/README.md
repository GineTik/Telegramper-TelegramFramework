# Executors and attributes
[to general readme.md](https://github.com/GineTik/TelegramFramework)

## Configure executors
For using executors, you should add ```builder.Services.AddExecutors()``` and ```app.UseExecutors()```.

Configure executors in services
```cs
builder.Services.AddExecutors(options =>
{
    // default values

    options.ParameterParser.DefaultSeparator = " ";
    options.ParameterParser.ErrorMessages.TypeParseError = "Parse type error";
    options.ParameterParser.ErrorMessages.ArgsLengthIsLess = "Args length is less";
    options.ParameterParser.ParserType = typeof(ParametersParser);

    options.UserState.DefaultUserState = "";
    options.UserState.SaverType = typeof(MemoryUserStateSaver);
});
```
and you can configure assemblies where the executors located
```cs
builder.Services.AddExecutors(new[] { Assembly.GetEntryAssembly()! }); // in the second parameter to set the default values as above
```

## Executors
Executor is basic abstract class who provide properties and methods. Executor has UpdateContext (identical to the HttpContext), Client (for send responce to a user), ExecuteAsync method (for execute other methods of executors).

### Executor structur
```cs
public class ExecutorName : Executor // the name does not affect anything
{
    [TargetAttribute...(...)]
    [ValidationAttribute]
    public async Task Start(string? param1, int param2) // the name of methods does not affect anything too, but the return type should be Task
    {
        // ... do anything
        await Client.SendTextMessageAsync($"Response"); // send a response
    }
}
```

### Executor infrastructure 
Executor contains:
- UpdateContext, [about this](https://github.com/GineTik/TelegramFramework/tree/master/Telegram.Framework/TelegramBotApplication/Context)
- ExecuteAsync method for invoke other methods other executors.

You can take dependencies from the constructor. 

### Executor parameters
You can take a parameters (with basic type and basic nullable type) from Message and CallbackQuery UpdateTypes. Nullable type is not required parameter (example below).
If UpdateType is equal to Message, then the Text property must be filled in the request. The command at the beginning will be removed from the text.
If UpdateType is equal to CallbackQuery, then the Data property must be filled in the request. In the data property, the first word is required for routing, the rest will be split into parameters

#### Attributes for parameters
- ```ParametersSeparatorAttribute(separator)```, the default is space(" ")
- ```EmptyParametersSeparatorAttribute```, set the separator to "", which means that can be one parameters, for example, /command param1 param2 param3, all after /command set into first parameter
- ```ParseErrorMessages```, set error messages that are sent to the user in response

#### Example:
Let's look at a few cases:
- Case 1: /example param1 2 2
- Case 2: /example
- Case 3: /example param1 param2
- Case 4: /example 10
- Case 5: /example 10 10

Code:
```cs
public class ExampleExecutor : Executor
{
    [TargetCommands("example")]
    [ParseErrorMessages(ArgsLengthIsLess = "ArgsLengthIsLess", TypeParseError = "TypeParseError")] // change the default error messages that are sent to the user in response
    public async Task Example(string? param1, int param2, int? param3)
    {
        await Client.SendTextMessageAsync($"all good, param1 is {param1 ?? "null"}, param2 is {param2}, param3 is {param3?.ToString() ?? "null"}");
    }
}
```

Results:
- Case 1: all good, each parameter has it's own values
- Case 2: the method is not executed, becouse the required parameter is param2 don't exists.
- Case 3: the method is not executed, becouse cannot parse string "param2" into int for param2.
- Case 4: all good, param1 is null, param2 is 10, param3 is null
- Case 5: all good, param1 is "10", param2 is 10, param3 is null

![Image example](https://github.com/GineTik/TelegramFramework/blob/master/Images/ExecutorsAndAttributes/ExampleOfParameters.png)


## Attributes

### Routing
There are target attributes for routing. You can attach one or more target attributes to a "handle" method.
> The method must return a Type as Task!

If at least one target attribute in the handler method matches, the method is executed.

### Available attributes for routing
- TargetCommands
  ```cs
  [TargetCommands("command1, commmand2, command3", Description = "Commands")]
  public async Task Handle() { }
  ```
- TargetCallbackData
  ```cs
  [TargetCallbacksDatas("data1, data2, data3")]
  public async Task Handle() { }
  ```
- TargetUpdateType
  ```cs
  [TargetUpdateType(UpdateType.Message)]
  public async Task Handle() { }
  ```
- TargetUserStateContains
  ```cs
  [TargetUserStateContains("userState1, userState2, userState3")]
  public async Task Handle() { }
  ```

This attributes checks the input data on similarity and attempts to execute the method if it is simiral. There can be more than one TargetAttributes per handler.

### Available attributes for input data validation
- RequiredUser
- RequiredChat
- RequiredMessageText
- RequiredMessagePhoto

```cs
[TargetAttribute...]
[RequiredAttribute...(ErrorMessage="error message")]
public async Task Handle() { }
```

Validation attributes don't executing Executor method if input data not correct. If validation is failed, runing next middleware. One handler can have more than one ValidationAttributes.

### Write your own attribute
Inherit the TargetAttribute or ValidateInputDataAttribute attribute and implement the method.
> !!! For TargetAttribute, you can add ```[TargetUpdateType(UpdateType.CallbackQuery)]```, then the routing will be faster.

For example
```cs
[TargetUpdateType(UpdateType.CallbackQuery)] // if you don't add this attribute, the default is UpdateType.Unknown
public class TargetCallbacksDatasAttribute : TargetAttribute
{
    public string[] CallbacksDatas { get; set; }

    public TargetCallbacksDatasAttribute(string callbacksDatas)
    {
        CallbacksDatas = callbacksDatas.Replace(" ", "").Split(",");
    }

    public override bool IsTarget(Update update)
    {
        if (update.CallbackQuery!.Data is not { } data)
            return false;

        var targetData = data.Split(' ').First();
        return CallbacksDatas.Contains(targetData);
    }
}
```
