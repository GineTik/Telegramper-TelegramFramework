# UpdateContext and UpdateContextAccessor
[to general readme.md](../../README.md)
 
## UpdateContext
UpdateContext contains:
- Update
- Client (TelegramBotClient)
- CancellationToken

These properties have types from the Telegram.Bot library.

The Update contains information about the current request.
The Client has methods to call the Telegram API.
The CancellationToken with which you can stop receiving.
  
### Shortcodes
- ```UpdateContext.User``` and ```UpdateContext.TelegramUserId```
  <details><summary>Available when UpdateType</summary>
  <br>
    
  - Message
  - CallbackQuery
  - EditedMessage
  - ChannelPost
  - EditedChannelPost
  - InlineQuery
  - PollAnswer
  - PreCheckoutQuery
  - ShippingQuery
  - ChatJoinRequest
  - ChatMember
  - MyChatMember
  <br>
  
  > ***The sender, empty if messages are sent to the channels***
    <br>
</details>
  
- ```UpdateContext.Chat``` and ```UpdateContext.ChatId```
  <details><summary>Available when UpdateType</summary>
  
    <br>

  - Message
  - CallbackQuery
  - EditedMessage
  - ChannelPost
  - EditedChannelPost
  - ChatJoinRequest
  - ChatMember
  - MyChatMember
  
  <br>
  
  > ***For UpdateType.CallbackQuery, the message content and message date will not be available if the message is too old***
    <br>
</details>

- ```UpdateContext.Message``` and ```UpdateContext.MessageId```
  <details><summary>Available when UpdateType</summary>
    
    <br>
    
  - Message
  - CallbackQuery
  - EditedMessage
  - ChannelPost
  - EditedChannelPost
  <br>
  
   > ***For UpdateType.CallbackQuery, the message content and message date will not be available if the message is too old***
  <br>
  </details>

## UpdateContextAccessor
The service that exists to save the UpdateContext. 

### For example
```cs
public class YourService : IYourService
{
    private readonly UpdateContext _updateContext;

    public ExecutorName(UpdateContextAccessor accessor)
    {
        _updateContext = accessor.UpdateContext;
    }

    public object DoSomthing()
    {
        // use _updateContext here
    }
}
```
