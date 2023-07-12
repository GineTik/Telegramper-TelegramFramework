using Telegram.Framework.TelegramBotApplication.AdvancedBotClient;
using Telegram.Bot.Types;

namespace Telegram.Framework.TelegramBotApplication.Context
{
    public class UpdateContext
    {
        public IAdvancedTelegramBotClient Client { get; set; } = default!;
        public Update Update { get; set; } = default!;
        public CancellationToken CancellationToken { get; set; } = default!;

        public Message Message => Update.Message ??
                                Update.CallbackQuery?.Message ??
                                Update.EditedMessage ??
                                Update.ChannelPost ??
                                throw new InvalidDataException("Don't found Message");

        public User User => Update.Message?.From ??
                            Update.CallbackQuery?.From ??
                            Update.EditedMessage?.From ??
                            Update.ChannelPost?.From ??
                            throw new InvalidDataException("Don't found User");

        public Chat Chat => Update.Message?.Chat ??
                            Update.CallbackQuery?.Message?.Chat ??
                            Update.EditedMessage?.Chat ??
                            Update.ChannelPost?.Chat ??
                            throw new InvalidDataException("Don't found Chat");

        public long ChatId => Chat.Id;
        public long TelegramUserId => User.Id;
        public int MessageId => Message.MessageId;
    }
}
