# Executors and attributes
[to general readme.md](https://github.com/GineTik/TelegramFramework)

## Configure executors
For using executors, you should add ```builder.Services.AddExecutors()``` and ```app.UseExecutors()```.

Configure executors in services
```cs
builder.Services.AddExecutors();
```
or
```cs
builder.Services.AddExecutors(options =>
{
    options.ParameterParser.DefaultSeparator = " ";
    options.ParameterParser.ErrorMessages.TypeParseError = "Parse type error";
    options.ParameterParser.ErrorMessages.ArgsLengthIsLess = "Args length is less";

    options.UserState.DefaultUserState = "";
});
```

Default values of executor options:
```cs
public class ExecutorOptions
{
    public IEnumerable<SmartAssembly> Assemblies { get; set; } = new[]
    {
        new SmartAssembly(Assembly.GetEntryAssembly()!)
    };
    
    public CommandExecutorOptions MethodNameTransformer { get; set; } = new()
    {
        NameTransformerType = typeof(SnakeCaseNameTransformer) // implementation INameTransformer
    };

    public ParametersParserOptions ParametersParser { get; set; } = new()
    {
        ParserType = typeof(ParametersParser), // implementation IParametersParser
        ErrorHandlerStrategyType = typeof(DefaultParseErrorStrategy), // implementation IParseErrorStrategy
        DefaultSeparator = ParametersParserOptions.NoneSeparator,
        ErrorMessages = new ParseErrorMessagesAttribute
        {
            TypeParseError = "Type parse error",
            ArgsLengthIsLess = "Args length is less"
        },
        ParameterParseStrategyType = typeof(DefaultParseStrategy) // implementation IParametersParseStrategy
    };
        
    public UserStateOptions UserState { get; set; } = new()
    {
        DefaultUserState = "",
        SaverType = typeof(MemoryUserStateSaveStrategy) // implementation IUserStateSaveStrategy
    };

    public HandlerQueueOptions HandlerQueue { get; set; } = new()
    {
        LimitOfHandlersPerRequest = 1
    };
}
```

## Executors
Executor is basic abstract class who provide properties and methods. 
Executor contains:
- UpdateContext ([about this](https://github.com/GineTik/Telegramper-TelegramFramework/blob/master/Documentation/Core/UpdateContext.md))
- Client (taked from UpdateContext)
- ServiceProvider (for take a dependencies)
- ExecuteAsync (method for invoke other methods other executors)

### Executor structur
```cs
public class ExecutorName : Executor // the name does not affect anything
{
    [Attributes] // TargetAttributes, ValidationAttributes, FilterAttributes (read more below)
    public async Task Method(parameters) //  the method should be async and the returned value should be Task type
    {
        // do anything
        await Client.SendTextMessageAsync($"Response"); // send a response
    }
}
```

### Executor parameters
You can take a parameters (with basic type and basic nullable type) from Message and CallbackQuery UpdateTypes. Nullable type is not required parameter (example below).
If UpdateType is equal to Message, then the Text property must be filled in the request. The command at the beginning will be removed from the text.
If UpdateType is equal to CallbackQuery, then the Data property must be filled in the request. In the data property, the first word is required for routing, the rest will be split into parameters

#### Attributes for parameters
- ```ParametersSeparatorAttribute(separator)```,
- ```EmptyParametersSeparatorAttribute```, set the separator to "", which means that can be only one parameters, for example, a user send /command param1 param2 param3, then param1 param2 param3 set into one parameter
- ```ParseErrorMessages```, set error messages that are sent to the user in response

 ```cs
  [TargetCommand]
  [ParseErrorMessages(your error messages after unsuccessful parameter parsing)]
  public async Task Echo(string phrase) // default missing separator
  {
      await Client.SendTextMessageAsync(phrase);
  }
  ```

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
    [ParametersSeparatorAttribute(" ")]
    [ParseErrorMessages(ArgsLengthIsLess = "ArgsLengthIsLess", TypeParseError = "TypeParseError")] // change the default error messages that are sent to the user in response
    public async Task Method(string? param1, int param2, int? param3)
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
- ```TargetCommand(command without a slash)```
- ```TargetCallbackData(callback data)```
- ```TargetUpdateType(UpdateType)```
- ```TargetTextAttribute(text, TextMatchingMode)```

For ```TargetCommand``` and ```TargetCallbackData``` attributes, you can not specify parameters, then the name of the method will be substituted in the parameters and will be changed using the INameTransformer implementation, by default it is SnakeCaseNameTransformer (you can change implementation of INameTransformer in options, read about it above)

This attributes checks the input data and accept to execute the method if validation result is successfully.

### Available attributes for input data validation
- ```RequiredData(UpdateProperty, error message)```

<details><summary>Available values of UpdateProperty</summary>

```cs
public enum UpdateProperty
{
    User,
    Chat,
    MessageText,
    MessagePhoto,
}
```

</details>

Validation attributes don't allow to executing method if input data not correct. If validation is failed, runing next middleware. One handler can have more than one ValidationAttributes.

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
            return targetData == MethodName;
        }

        return CallbackDatas.Contains(targetData);
    }
}
```
