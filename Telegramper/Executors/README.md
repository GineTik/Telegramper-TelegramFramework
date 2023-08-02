# Executors and attributes
[to general readme.md](https://github.com/GineTik/TelegramFramework)

## Configure executors
For using executors, you should add ```builder.Services.AddExecutors()``` and ```app.UseExecutors()```.

Configure executors in services
```cs
builder.Services.AddExecutors(options =>
{
    // default values

    options.MethodNameTransformer.Type = typeof(SnakeCaseNameTransformer); // the class that transform the method name for use [TargetCommands] and [TargetCallbackData] without parameters

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
- UpdateContext, [about this](https://github.com/GineTik/TelegramFramework/tree/master/Telegramper/TelegramBotApplication/Context)
- ExecuteAsync method for invoke other methods other executors.

You can take dependencies from the constructor. 

### Executor parameters
You can take a parameters (with basic type and basic nullable type) from Message and CallbackQuery UpdateTypes. Nullable type is not required parameter (example below).
If UpdateType is equal to Message, then the Text property must be filled in the request. The command at the beginning will be removed from the text.
If UpdateType is equal to CallbackQuery, then the Data property must be filled in the request. In the data property, the first word is required for routing, the rest will be split into parameters

#### Attributes for parameters
- ```ParametersSeparatorAttribute(separator)```, the default is space(" ")
- ```EmptyParametersSeparatorAttribute```, set the separator to "", which means that can be only one parameters, for example, a user send /command param1 param2 param3, then param1 param2 param3 set into one parameter
  ```cs
  [TargetCommands("echo")]
  [EmptyParameterSeparator] // remove separator
  public async Task Echo(string phrase)
  {
      await Client.SendTextMessageAsync(phrase);
  }
  ```
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
- TargetText
  ```cs
  [TargetText("target text")]
  public async Task Handle() { }
  ```
- TargetContainsText
  ```cs
  [TargetContainsText("target contains text")]
  public async Task Handle() { }
  ```

This attributes checks the input data on similarity and attempts to execute the method if it is simiral. There can be more than one TargetAttributes per handler.

### Available attributes for input data validation
- RequireUser
- RequireChat
- RequireMessageText
- RequireMessagePhoto

```cs
[TargetAttribute...]
[RequireAttribute...(ErrorMessage="error message")]
public async Task Handle() { }
```

Validation attributes don't executing Executor method if input data not correct. If validation is failed, runing next middleware. One handler can have more than one ValidationAttributes.

### Write your own attribute
Inherit the TargetAttribute or ValidateInputD ataAttribute attribute and implement the method.
> For your own TargetAttribute, you can add ```[TargetUpdateType(UpdateType...)]```, then your attribute will not run if the UpdateType of the update is not equal to the one set on your own attribute. If your attribute's UpdateType is set to Unknown, then it will always run. If you set TargetUpdateType, then routing will be faster.

> TargetAttribute has MethodName and TransformedMethodName properties, TransformedMethodName contains the transformed method name using INameTransformer (you can customize the INameTransformer implementation in ```AddExecutors(options => options.MethodNameTransformer.Type = typeof(SnakeCaseNameTransformer))```.

For example

```cs
[TargetUpdateType(UpdateType.CallbackQuery)] // if you don't add this attribute, the default is UpdateType.Unknown
public class TargetCallbackDatasAttribute : TargetAttribute
{
    public string[] CallbacksDatas { get; set; }

    public TargetCallbackDatasAttribute(string? callbacksDatas = null)
    {
        CallbackDatas = callbacksDatas?.Replace(" ", "").Split(",")
            ?? new string[0];
    }

    public override bool IsTarget(Update update)
    {
        var data = update.CallbackQuery!.Data;
        if (data == null)
        {
            return false;
        }

        var targetData = data.Split(' ').First();
        if (CallbackDatas.Length == 0)
        {
            return targetData == TransformedMethodName;
        }

        return CallbackDatas.Contains(targetData);
    }
}
```
