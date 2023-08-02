using Telegramper.TelegramBotApplication.AdvancedBotClient;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramper.TelegramBotApplication.Exceptions;

namespace Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions
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
            var chatId = client.UpdateContext.ChatId;
            if (chatId == null)
                throw new MessageMayBeTooOld();

            return await client.SendTextMessageAsync(
                chatId,
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
