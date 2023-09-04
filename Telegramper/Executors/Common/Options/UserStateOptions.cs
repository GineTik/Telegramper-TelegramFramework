using Telegramper.Executors.Common.Exceptions;
using Telegramper.Executors.QueryHandlers.UserState.Saver;
using Telegramper.Executors.QueryHandlers.UserState.Saver.Implementations;

namespace Telegramper.Executors.Common.Options
{
    public class UserStateOptions
    {
        public string DefaultUserState { get; set; } = "";
        private Type _saverType = typeof(MemoryUserStateSaver);
        public Type SaverType
        {
            get
            {
                return _saverType;
            }
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IUserStateSaver>(value);
                _saverType = value;
            }
        }
    }
}
