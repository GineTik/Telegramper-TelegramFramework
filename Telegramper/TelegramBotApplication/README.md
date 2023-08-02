# Configuration bot in Program.cs
How said above, Program.cs similar on Program.cs from .Net 7. WebApplicationBuilder and IApplication is a BotApplicaitionBuilder and BotApplication in the Telegram Framework. BotApplicationBuilder has Configuration property.

Scoped services will be not work. Configuration also based on appsettings.json.

## Quick configuration
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

## BotApplicationBuilder 

### Configure api key
To configure the api key you can use ```builder.ConfigureApiKey("your api key")```
You can set the api key in the appsettings.json file. In this case, the api key is installed automatically.
```json
{
   "ApiKey": "your api key"
}
```

### Configuration
To use the configuration, you need to create the appsettings.json file in your project at the same level as Program.cs. If the appsetting.json is not created, you will receive an exception at startup. The configuration is also identical to ASP.Net Core.


### ReceiverOptions
Availible methods:
- ```ConfigureAllowedUpdates(params UpdateType[] allowedUpdates)```
- ```Configure(Action<ReceiverOptions> configure)```

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


### Services
The functionality of services is taken over by IServiceCollection (from ASP.Net Core). Because of this, some services are not available, but you can get other services (which are not only available for ASP.Net Core), such as AutoMapper, EF Core ([How to use EF Core in Telegramper](#)), and others. Although these services were developed for ASP.Net Core, you can use them here as well. 
> You can also create your own services and add them to nuget packages to extend the functionality of the framework.

<br>

## IBotApplication
IBotApplication has next methods:
- ```Use(Func<UpdateContext, NextDelegate, Task> middlware)```
- ```Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware)```
- ```UseMiddleware<T>() where T : class, IMiddleware```
- ```RunPolling()```
Each Use methods return IBotApplication.


### Middleware example
```cs
var app = builder.Build()
   .UseOne()
   .UseTwo()
   .UseThree()
   .UseMiddleware<CustomMiddleware>()
   .Use(async (serviceProvider, updateContext, next) =>
   {
      var service = serviceProvider.GetRequiredService<SomeService>();
      // ...
      await next();
   });
```

### Write your own middleware
To write your own middleware, you must implement the IMiddleware interface or write a lambda to the Use method.

#### Implement the IMiddleware interface
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
#### Write a lambda to the Use method.
```cs
app.Use(async (serviceProvider, updateContext, next) =>
{
   var service = serviceProvider.GetRequiredService<SomeService>();
   // ...
   await next.Invoke(); // to call next middleware
});
```

### Launch your bot
To start the bot, you need to call the Run method. You can call a polling using the RunPolling method.
> In the future, we plan to create the RunWebhooks method.
