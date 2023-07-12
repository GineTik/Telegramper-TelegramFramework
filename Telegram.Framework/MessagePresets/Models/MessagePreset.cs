using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Framework.MessagePresets.Models
{
    public class MessagePreset
    {
        public string Text { get; set; }
        public int? MessageThreadId { get; set; }
        public ParseMode? ParseMode { get; set; }
        public IEnumerable<MessageEntity>? Entities { get; set; }
        public bool? DisableWebPagePreview { get; set; }
        public bool? DisableNotification { get; set; }
        public bool? ProtectContent { get; set; }
        public int? ReplyToMessageId { get; set; }
        public bool? AllowSendingWithoutReply { get; set; }
        public IReplyMarkup? ReplyMarkup { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
