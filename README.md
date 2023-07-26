# Telegram Framework

It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

> The framework is under development, so unexpected errors, changes in functionality, and names are possible! I would be grateful if you could report any bugs or functionality you need.

## Content
1. [Quick start](#quick-start)
1. [Configuration bot in Program.cs](#configuration-bot)
   - [Quick configuration](#quick-configuration)
   - [BotApplicationBuilder](#BotApplicationBuilder)
      - [Configure api key](#Configure-api-key)
      - [Configuration](#Configuration)
      - [ReceiverOptions](#ReceiverOptions)
      - [Services](#services)
   - [IBotApplication](#IBotApplication)
      - [Middleware examples](#middleware-example)
      - [Write your own middleware](#write-your-own-middleware)
      - [Launch your bot](#launch-your-bot)
1. [Executors and attributes](#executors-and-attributes)
   - [Executors](#executors)
      - [Basic executor](#basic-executor)
   - [Attributes](#attributes)
      - [Routing](#routing-by-executors-and-attributes)
      - [Available attributes for routing](#available-attributes-for-routing)
      - [Available attributes for input data validation](#available-attributes-for-input-data-validation)
      - [Write your own attributes](#write-your-own-attribute)

<a name="quick-start"></a>
## Quick start
```cs
internal class Program
{
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
}

public class BasicExecutor : Executor
{
    [TargetCommands("start", Description = "start command")]
    public async Task Start()
    {
        var sender = UpdateContext.User.ToString();
        await Client.SendTextMessageAsync($"You are {sender}"); // send a text response
    }

    [TargetCommands("echo, command2", Description = "Echo description")]
    [EmptyParameterSeparator] // remove separator, by default is space(" ")
    public async Task Echo(string phrase) // more about the parameters later 
    {
        await Client.SendTextMessageAsync(phrase);
    }
}
```
<br>
<br>

<a name="configuration-bot"></a>
## Configuration bot in Program.cs
How said above, Program.cs similar on Program.cs from .Net 7. WebApplicationBuilder and IApplication is a BotApplicaitionBuilder and BotApplication in the Telegram Framework. BotApplicationBuilder has Configuration property.

Scoped services will be not work. Configuration also based on appsettings.json.

<a name="quick-configuration"></a>
### Quick configuration
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
<br>

### BotApplicationBuilder 

#### Configure api key
To configure the api key you can use ```builder.ConfigureApiKey("your api key")```
You can set the api key in the appsettings.json file. In this case, the api key is installed automatically.
```json
{
   "ApiKey": "your api key"
}
```

#### Configuration
To use the configuration, you need to create the appsettings.json file in your project at the same level as Program.cs. If the appsetting.json is not created, you will receive an exception at startup. The configuration is also identical to ASP.Net Core.

#### ReceiverOptions
Availible methods
```cs
ConfigureAllowedUpdates(params UpdateType[] allowedUpdates)
Configure(Action<ReceiverOptions> configure)
```

ReceiverOptions model
```cs
public sealed class ReceiverOptions
{
   public int? Offset { get; set; }
   public UpdateType[]? AllowedUpdates { get; set; } // default is [ UpdateType.Message ]
   public int? Limit // default is 0
   public bool ThrowPendingUpdates { get; set; }
}
```

#### Services
The functionality of services is taken over by IServiceCollection (from ASP.Net Core). Because of this, some services are not available, but you can get other services (which are not only available for ASP.Net Core), such as AutoMapper, EF Core ([How to use EF Core in Telegram.Framework](#)), and others. Although these services were developed for ASP.Net Core, you can use them here as well. 
> You can also create your own services and add them to nuget packages to extend the functionality of the framework.

### IBotApplication
IBotApplication has next methods:
- ```Use(Func<UpdateContext, NextDelegate, Task> middlware)```
- ```Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)```
- ```UseMiddleware<T>() where T : class, IMiddleware```
- ```RunPolling()```
Each Use methods return IBotApplication.

<a name="middleware-example"></a>
#### Middleware example
```cs
var app = builder.Build()
   .UseOne()
   .UseTwo()
   .UseThree()
   .UseMiddleware<CustomMiddleware>()
   .Use(async (updateContext, next) =>
   {
      // ...
      await next();
   });
```

#### Write your own middleware
To write your own middleware, you must implement the IMiddleware interface or write a lambda to the Use method.

##### Implement the IMiddleware interface
```cs
public class CustomMiddleware : IMiddleware
{
   private readonly YourDependence _dependence;
   
   public CustomMiddleware(YourDependence dependence)
   {
      _dependence = dependence;
   }
   
   public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
   {
      // ...
      await next();
   }
}
```
##### Write a lambda to the Use method.
```cs
app.Use((updateContext, next) =>
{
   // ...
   next.Invoke(); // to call next middleware
});
```

#### Launch your bot
To start the bot, you need to call the Run method. You can call a polling using the RunPolling method.
> In the future, we plan to create the RunWebhooks method.

<br>
<br>

<a name="executors-and-attributes"></a>
## Executors and attributes

### Executors
Executor is basic abstract class who provide properties and methods. Executor has UpdateContext (identical to the HttpContext), Client (for send responce to a user), ExecuteAsync method (for execute other methods of executors).

#### Basic executor
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

<br>

### Attributes

<a name="routing-by-executors-and-attributes"></a>
#### Routing
There are target attributes for routing. Learn about these attributes [here](#available-attributes-for-routing). You can attach one or more target attributes to a processing method.
> The method must return a Type as Task!

If at least one target attribute in the handler method matches, the method is executed.

<a name="available-attributes-for-routing"></a>
#### Available attributes for routing
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
#### Available attributes for input data validation
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
#### Write your own attribute
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
