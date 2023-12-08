using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.QueryHandlers.UserState.Saver;
using Telegramper.Executors.QueryHandlers.UserState.Saver.Implementations;

namespace Telegramper.Executors.Common.Options
{
    public class UserStateOptions
    {
        /// default values see in <see cref="ExecutorOptions"/>
        
        private Type _saverType = null!;
        public required Type SaverType
        {
            get => _saverType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IUserStateSaver>(value);
                _saverType = value;
            }
        }
        
        public required string DefaultUserState { get; set; }
    }
}
