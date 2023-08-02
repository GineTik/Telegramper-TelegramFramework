using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Saver;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Sessions.Implementations
{
    public class UserSession : Session, IUserSession
    {
        public UserSession(ISessionDataSaver saver, UpdateContextAccessor updateContextAccessor) : base(saver, updateContextAccessor)
        {
        }

        protected override long GetCurrentEntityId(UpdateContext updateContext)
        {
            return updateContext.TelegramUserId
                ?? throw new NullReferenceException("UpdateContext.TelegramUserId is null");
        }
    }
}
