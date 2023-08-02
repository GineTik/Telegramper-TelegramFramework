using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.TelegramBotApplication.AdvancedBotClient;

namespace Telegramper.TelegramBotApplication.Context
{
    public class UpdateContext
    {
        public IAdvancedTelegramBotClient Client { get; set; } = default!;
        public Update Update { get; set; } = default!;
        public CancellationToken CancellationToken { get; set; } = default!;

        private Message? _message;
        /// <summary>
        /// UpdateType.CallbackQuery. Message content and message date will not be available if the message is too old
        /// </summary>
        public Message? Message => _message ??= Update.Type switch
        {
            UpdateType.Message => Update.Message,
            UpdateType.CallbackQuery => Update.CallbackQuery?.Message,
            UpdateType.EditedMessage => Update.EditedMessage,
            UpdateType.ChannelPost => Update.ChannelPost,
            UpdateType.EditedChannelPost => Update.EditedChannelPost,
            _ => null
        };

        private User? _user;
        /// <summary>
        /// The sender, empty if messages are sent to the channels
        /// </summary>
        public User? User => _user ??= Update.Type switch
        {
            UpdateType.Message => Update.Message!.From,
            UpdateType.CallbackQuery => Update.CallbackQuery!.From,
            UpdateType.EditedMessage => Update.EditedMessage!.From,
            UpdateType.ChannelPost => Update.ChannelPost!.From,
            UpdateType.EditedChannelPost => Update.EditedChannelPost!.From,
            UpdateType.InlineQuery => Update.InlineQuery!.From,
            UpdateType.PollAnswer => Update.PollAnswer!.User,
            UpdateType.PreCheckoutQuery => Update.PreCheckoutQuery!.From,
            UpdateType.ShippingQuery => Update.ShippingQuery!.From,
            UpdateType.ChatJoinRequest => Update.ChatJoinRequest!.From,
            UpdateType.ChatMember => Update.ChatMember!.From,
            UpdateType.MyChatMember => Update.MyChatMember!.From,
            _ => null
        };
        
        private Chat? _chat;
        /// <summary>
        /// UpdateType.CallbackQuery. Chat will not be available if the message is too old
        /// </summary>
        public Chat? Chat => _chat ??= Update.Type switch
        {
            UpdateType.Message => Update.Message!.Chat,
            UpdateType.CallbackQuery => Update.CallbackQuery!.Message?.Chat,
            UpdateType.EditedMessage => Update.EditedMessage!.Chat,
            UpdateType.ChannelPost => Update.ChannelPost!.Chat,
            UpdateType.EditedChannelPost => Update.EditedChannelPost!.Chat,
            UpdateType.ChatJoinRequest => Update.ChatJoinRequest!.Chat,
            UpdateType.ChatMember => Update.ChatMember!.Chat,
            UpdateType.MyChatMember => Update.MyChatMember!.Chat,
            _ => null
        };

        public long? ChatId => Chat?.Id;
        public long? TelegramUserId => User?.Id;
        public int? MessageId => Message?.MessageId;
    }
}
