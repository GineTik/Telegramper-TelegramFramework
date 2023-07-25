# Telegram Framework

It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

## Configuration bot in Program.cs
How said above, Program.cs similar on Program.cs from .Net 7. WebApplicationBuilder and IApplication is a BotApplicaitionBuilder and BotApplication in the Telegram Framework. BotApplicationBuilder has Configuration property.

Scoped services will be not work. Configuration also based on appsettings.json.

### Default configuration
```cs
static void Main(string[] args)
{
    var builder = new BotApplicationBuilder();
    builder.ConfigureApiKey("your api key");
    builder.ReceiverOptions.ConfigureAllowedUpdates(UpdateType.Message, UpdateType.CallbackQuery); // default is UpdateType.Message
    builder.Services.AddExecutors(); // identical to the controller in ASP.Net Core

    var app = builder.Build();
    app.UseExecutors();
    app.RunPolling(); // webhooks are not implemented, but in the future you will be able to, for example, change polling to webhooks and vice versa
}
```

## Routing
For the routing exists executers(identical to the controllers) and attributes.

Executor is basic abstract class who provide properties and methods. Executor has UpdateContext (identical to the HttpContext), Client (for send responce to a user), ExecuteAsync method (for execute other methods of executors).

###Examples
```cs
public class BasicExecutor : Executor
{
    [TargetCommands("start", Description = "start command")]
    public async Task Start()
    {
        var username = UpdateContext.User.ToString();
        await Client.SendTextMessageAsync($"Your username is {username}"); // send responce
    }

    [TargetCommands("params_examples, pe", Description = "Parameters examples")]
    public async Task ParametersExamples(string parameter1, int? parameter2) // more about the parameters later 
    {
        // ...
    }
}
```

### Available attributes for routing
- TargetCommands
  ```cs
  [TargetCommands("command1, commmand2, command3", Description = "Commands")]
  public async Task Handler() { }
  ```
- TargetCallbackData
  ```cs
  [TargetCallbacksDatas("data1, data2, data3", Description = "CallbackQueryData")]
  public async Task Handler() { }
  ```
- TargetUpdateType
  ```cs
  [TargetUpdateType(UpdateType.Message)]
  public async Task Handler() { }
  ```
- TargetUserStateContains
  ```cs
  [TargetUserStateContains("userState1, userState2, userState3")]
  public async Task Handler() { }
  ```

This attributes checks the input data on similarity and attempts to execute the method if it is simiral. There can be more than one TargetAttributes per handler.

### Available attributes for input data validation
- UpdateMessageTextNotNull
  ```cs
  [TargetAttribute...]
  [UpdateMessageTextNotNull(ErrorMessage="please send a text")] // by default ErrorMessage is "Test is null"
  public async Task Handler() { }
  ```
- UpdatePhotoNotNull
  ```cs
  [TargetAttribute...]
  [UpdatePhotoNotNull(ErrorMessage="please send a photo")] // by default ErrorMessage is "Photo is null"
  public async Task Handler() { }
  ```

Validation attributes don't executing Executor method if input data not correct. If validation is failed, runing next middleware. There can be more than one ValidationAttributes per handler.

### Write your own attribute
Inherit TargetAttribute or ValidateInputDataAttribute and implements method. 
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
