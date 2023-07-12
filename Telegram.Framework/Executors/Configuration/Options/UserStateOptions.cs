using Telegram.Framework.Executors.Helpers.Exceptions;
using Telegram.Framework.Executors.Storages.UserState.Saver;

namespace Telegram.Framework.Executors.Configuration.Options
{
    public class UserStateOptions
    {
        public string DefaultUserState { get; set; }

        private Type _saverType;
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
