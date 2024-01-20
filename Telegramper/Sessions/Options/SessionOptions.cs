using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.Common.Options;
using Telegramper.Sessions.Saver;

namespace Telegramper.Sessions.Options;

public class SessionOptions
{
    /// default values see in <see cref="ExecutorOptions"/>
    
    private Type _saveStrategyType = null!;
    public Type SaveStrategyType 
    {  
        get => _saveStrategyType;
        set
        {
            InvalidTypeException.ThrowIfNotImplementation<ISessionSaveStrategy>(value);
            _saveStrategyType = value;
        } 
    }
}