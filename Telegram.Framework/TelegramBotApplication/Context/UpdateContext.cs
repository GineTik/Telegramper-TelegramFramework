using Telegram.Framework.TelegramBotApplication.AdvancedBotClient;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Framework.TelegramBotApplication.Exceptions;

namespace Telegram.Framework.TelegramBotApplication.Context
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
        /// <exception cref="InvalidUpdateTypeException"></exception>
        public Message? Message => _message ??= Update.Type switch
        {
            UpdateType.Message => Update.Message,
            UpdateType.CallbackQuery => Update.CallbackQuery?.Message,
            UpdateType.EditedMessage => Update.EditedMessage,
            UpdateType.ChannelPost => Update.ChannelPost,
            UpdateType.EditedChannelPost => Update.EditedChannelPost,
            _ => throw new InvalidUpdateTypeException("Invalid UpdateType for using a property")
        };

        private User? _user;
        /// <summary>
        /// The sender, if you use this property, will throw exceptions if messages are sent to the channels
        /// </summary>
        /// <exception cref="InvalidUpdateTypeException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public User User => _user ??= Update.Type switch
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
            _ => throw new InvalidUpdateTypeException("Invalid UpdateType for using a property")
        } ?? throw new NullReferenceException("Sender is empty for messages sent to channels");
        
        private Chat? _chat;
        /// <summary>
        /// UpdateType.CallbackQuery. Chat will not be available if the message is too old
        /// </summary>
        /// <exception cref="InvalidUpdateTypeException"></exception>
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
            _ => throw new InvalidUpdateTypeException("Invalid UpdateType for using a property")
        };

        public long? ChatId => Chat?.Id;
        public long TelegramUserId => User.Id;
        public int? MessageId => Message?.MessageId;
    }
}
