# Telegram Framework

It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

## Configuration bot in Program.cs
How said above, Program.cs similar on Program.cs from .Net 7. WebApplicationBuilder and IApplication is a BotApplicaitionBuilder and BotApplication in the Telegram Framework. BotApplicationBuilder has Configuration property.

Scoped services will be not work. Configuration also based on appsettings.json.

### Default configuration
```
static void Main(string[] args)
{
    var builder = new BotApplicationBuilder();
    builder.ConfigureApiKey("your api key");
    builder.ReceiverOptions.ConfigureAllowedUpdates(UpdateType.Message, UpdateType.CallbackQuery);
    builder.Services.AddExecutors(); // identical to the controller in ASP.Net Core

    var app = builder.Build();
    app.UseExecutors();
    app.RunPolling(); // webhooks are not implemented, but in the future you will be able to, for example, change polling to webhooks and vice versa
}
```

## Routing
For the routing exists executers(identical to the controllers) and attributes.

Executor is basic abstract class who provide properties and methods. Executor has UpdateContext (identical to the HttpContext), Client (for send responce to a user), ExecuteAsync method (for execute other methods of executors).

### Available attributes for routing
- TargetCommands
- TargetCallbackData
- TargetUpdateType
- TargetUserState

This attributes checks the input data on similarity and attempts to execute the method if it is simiral.

### Available attributes for input data validation
- UpdateMessageTextNotNull
- UpdatePhotoNotNull

Validation attributes don't executing Executor method if input data not correct. If validation is failed, runing next middleware.
