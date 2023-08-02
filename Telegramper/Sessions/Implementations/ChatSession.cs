using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Saver;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Sessions.Implementations
{
    public class ChatSession : Session, IChatSession
    {
        public ChatSession(ISessionDataSaver saver, UpdateContextAccessor updateContextAccessor) : base(saver, updateContextAccessor)
        {
        }

        protected override long GetCurrentEntityId(UpdateContext updateContext)
        {
            return updateContext.ChatId
                ?? throw new NullReferenceException("UpdateContext.ChatId is null");
        }
    }
}
