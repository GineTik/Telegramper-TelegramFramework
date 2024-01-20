using Telegramper.Sessions.Interfaces;
using Telegramper.Sessions.Saver;
using Telegramper.Core.Context;

namespace Telegramper.Sessions.Implementations
{
    public class ChatSession : Session, IChatSession
    {
        public ChatSession(ISessionSaveStrategy saver, UpdateContextAccessor updateContextAccessor) : base(saver, updateContextAccessor)
        {
        }

        protected override string? BuildKey<T>()
        {
            return "Chat:" + typeof(T).Name;
        }

        protected override long GetCurrentEntityId(UpdateContext updateContext)
        {
            return updateContext.ChatId
                ?? throw new NullReferenceException("UpdateContext.ChatId is null");
        }
    }
}
