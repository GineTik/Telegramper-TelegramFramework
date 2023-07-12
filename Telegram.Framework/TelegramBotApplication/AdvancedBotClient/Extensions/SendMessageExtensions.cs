using Telegram.Framework.TelegramBotApplication.AdvancedBotClient;
using Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions
{
    public static class SendMessageExtensions
    {
        public static async Task<Message> SendTextMessageAsync(
            this IAdvancedTelegramBotClient client,
            string text,
            int? messageThreadId = default,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity>? entities = default,
            bool? disableWebPagePreview = default,
            bool? disableNotification = default,
            bool? protectContent = default,
            int? replyToMessageId = default,
            bool? allowSendingWithoutReply = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default)
        {
            return await client.SendTextMessageAsync(
                client.UpdateContext.ChatId,
                text,
                messageThreadId,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken);
        }
    }
}
