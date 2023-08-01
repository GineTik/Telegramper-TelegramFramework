using Telegramper.Executors.Helpers.Exceptions;
using Telegramper.Executors.Storages.UserState.Saver;

namespace Telegramper.Executors.Configuration.Options
{
    public class UserStateOptions
    {
        public string DefaultUserState { get; set; } = default!;

        private Type _saverType = default!;
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
