using Telegramper.Executors.Building.Exceptions;
using Telegramper.Executors.Routing.UserState.Saver;
using Telegramper.Executors.Routing.UserState.Saver.Implementations;

namespace Telegramper.Executors.Build.Options
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
