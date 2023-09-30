# Telegramper
It is framework similar to a ASP.Net Core. Framework contains services, middlewares, configuration, controllers(executors) and other.

> The framework is under development, so unexpected errors, changes in functionality, and names are possible! I would be grateful if you could report any bugs or functionality you need.

## Learn more about the framework
1. [About UpdateContext (similar to HttpContext)](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Telegramper/TelegramBotApplication/Context)
1. [Configuration bot in Program.cs (Services, Middlewares, etc.)](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Telegramper/TelegramBotApplication)
1. [Executors and attributes](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Telegramper/Executors)
1. [Session](https://github.com/GineTik/Telegramper-TelegramFramework/tree/master/Telegramper/Sessions)

### Chapters coming soon
- Dialog (comleted, documentation in process)
- Examples of projects written on the Telegramper framework

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
    [TargetCommand] // identical to [TargetCommand("start")]
    public async Task Start()
    {
        var sender = UpdateContext.User.ToString();
        await Client.SendTextMessageAsync($"You are {sender}"); // send a text response
    }

    [TargetCommand("echo, command2")]
    [EmptyParameterSeparator] // remove separator, by default is space(" ")
    public async Task Echo(string phrase) // more about the parameters later 
    {
        await Client.SendTextMessageAsync(phrase);
    }
}
```
