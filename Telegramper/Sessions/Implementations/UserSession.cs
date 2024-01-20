using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Saver;
using Telegramper.Core.Context;

namespace Telegramper.Sessions.Implementations
{
    public class UserSession : Session, IUserSession
    {
        public UserSession(ISessionSaveStrategy saver, UpdateContextAccessor updateContextAccessor) : base(saver, updateContextAccessor)
        {
        }

        protected override string? BuildKey<T>()
        {
            return "User:" + typeof(T).Name;
        }

        protected override long GetCurrentEntityId(UpdateContext updateContext)
        {
            return updateContext.TelegramUserId
                ?? throw new NullReferenceException("UpdateContext.TelegramUserId is null");
        }
    }
}
