# Telegram Framework

It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

## Content
1. [Configuration bot in Program.cs](#configuration-bot)
   - [Default configuration](#default-configuration)
1. [Executors and attributes](#executors-and-attributes)
   - [Routing](#routing-by-executors-and-attributes)
   - [Available attributes for routing](#available-attributes-for-routing)
   - [Available attributes for input data validation](#available-attributes-for-input-data-validation)
   - [Write your own attributes](#write-your-own-attribute)

<a name="configuration-bot"></a>
## Configuration bot in Program.cs
How said above, Program.cs similar on Program.cs from .Net 7. WebApplicationBuilder and IApplication is a BotApplicaitionBuilder and BotApplication in the Telegram Framework. BotApplicationBuilder has Configuration property.

Scoped services will be not work. Configuration also based on appsettings.json.

<a name="default-configuration"></a>
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

<a name="executors-and-attributes"></a>
## Executors and attributes
Executor is basic abstract class who provide properties and methods. Executor has UpdateContext (identical to the HttpContext), Client (for send responce to a user), ExecuteAsync method (for execute other methods of executors).

<a name="routing-by-executors-and-attributes"></a>
### Routing
There are target attributes for routing. Learn about these attributes [here](#available-attributes-for-routing). You can attach one or more target attributes to a processing method.
> The method must return a Type as Task!

If at least one target attribute in the handler method matches, the method is executed.

<a name="available-attributes-for-routing"></a>
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

<a name="available-attributes-for-input-data-validation"></a>
### Available attributes for input data validation
- UpdateMessageTextNotNull
  ```cs
  [TargetAttribute...]
  [UpdateMessageTextNotNull(ErrorMessage="please send a text")] // by default ErrorMessage is "Test is null"
  public async Task Handle() { }
  ```
- UpdatePhotoNotNull
  ```cs
  [TargetAttribute...]
  [UpdatePhotoNotNull(ErrorMessage="please send a photo")] // by default ErrorMessage is "Photo is null"
  public async Task Handle() { }
  ```

Validation attributes don't executing Executor method if input data not correct. If validation is failed, runing next middleware. There can be more than one ValidationAttributes per handler.

<a name="write-your-own-attribute"></a>
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

### Basic executor
```cs
public class BasicExecutor : Executor
{
    [TargetCommands("start", Description = "start command")]
    public async Task Start()
    {
        var username = UpdateContext.User.ToString();
        await Client.SendTextMessageAsync($"Your username is {username}"); // send response
    }

    [TargetCommands("params_examples, pe", Description = "Parameters examples")]
    public async Task ParametersExamples(string parameter1, int? parameter2) // more about the parameters later 
    {
        // ...
    }
}
```
