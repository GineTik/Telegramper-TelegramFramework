# Session

## Configuration session

To use a session, following below:
```cs
// ...
builder.Services.AddSessions();
// ...
```

The ISessionDataSaver implementation is used to save session data, the default is MemorySessionDataSaver, but you can write your own implementation as follows:
```cs
public class YourSessionDataSaver : ISessionDataSaver
{
  public async Task SetAsync<T>(long entityId, string key, T data)
  {
    // ...
  }

  public async Task<T?> GetAsync<T>(long entityId, string key)
  {
    // ...
  }

  public async Task RemoveAsync(long entityId, string key)
  {
    // ...
  }
}
```
And add you implementation to services:
```cs
// ...
builder.Services.AddSessions<YourSessionDataSaver>();
// ...
```

## How to use session

There are two classes of sessions: UserSession and ChatSession. The first one saves data for the user and uses TelegramUserId, the second one is for the chat and uses ChatId. 
If the user id or chat id does not exist, an exception is thrown.

The use of UserSession and ChatSession is the same (the only difference is the use of different identifiers).

Availible methods for get and set the session datas (you can use this methods with UserSession and ChatSession):
- ```GetAndRemoveAsync<T>(long? entityId = null, string? key = null);```
- ```GetAsync<T>(long? entityId = null, string? key = null);```
- ```GetAsync<T>(T defaultValue, long? entityId = null, string? key = null);```
- ```SetAsync<T>(T value, long? entityId = null, string? key = null);```
- ```SetAsync<T>(Action<T?> changeValues, long? entityId = null, string? key = null);```
- ```SetAsync<T>(T defaultValue, Action<T> changeValues, long? entityId = null, string? key = null);```
- ```RemoveAsync<T>(long? entityId = null, string? key = null);```

It is recommended to use methods with the devaultValue parameter unless you are sure that data with the key is already stored in the session.

If no key is given, the name of the class the method was typed with will be used (GetAsync<T>, typeof(T).Name will be the key)
If entityId is not set, the id (TelegramUserId or ChatId) in the current request will be used.
DefaultValue - the value that is returned if there is no data for this key in the session.

### Example using executors
```cs
public class TestsExecutor : Executor
{
  private readonly IUserSession _userSession; // use IChatSession in the same way

  public TestsExecutor(IUserSession userSession, IUserStateStorage state)
  {
      _userSession = userSession;
      _state = state;
  }

  [TargetCommands]
  [RequireChat]
  public async Task Start()
  {
      await _userSession.SetAsync(new SomeClass
      {
          Somethings = // ...
      });
      var value = await _userSession.GetAsync<SomeClass>();
      await Client.SendTextMessageAsync("send");
  }
}
```

### Example using default value
get with default value
```cs
await _userSession.GetAsync<SomeClass>(defaultValue: new SomeClass());
// if the session not contains SomeClass data, will be returned default value
```
set with default value
```cs
await _userSession.SetAsync(new SomeClass(), someClass => someClass.Something = ...);
```
this set is wrapper for next code
```cs
var value = await GetAsync(defaultValue, entityId, key);
changeValues(value);
await SetAsync(value, entityId, key);
```

### Example using custom key
```cs
await _userSession.SetAsync(new SomeClass() { Something = 1 }, key: "custom key");
await _userSession.SetAsync(new SomeClass() { Something = 2 }, key: "custom key 2");

var value = await _userSession.GetAsync<SomeClass>(); // return null
var value = await _userSession.GetAsync<SomeClass>(key: "custom key"); // return SomeClass with Something 1
var value = await _userSession.GetAsync<SomeClass>(key: "custom key 2"); // return SomeClass with Something 2
```
