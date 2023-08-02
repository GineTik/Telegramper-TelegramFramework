using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegramper.TelegramBotApplication.Exceptions;

namespace Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions
{
    public static class EditMessageExtensions
    {
        public static async Task EditMessageTextAsync(
            this IAdvancedTelegramBotClient client,
            string text,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity>? entities = default,
            bool? disableWebPagePreview = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default)
        {
            var chatId = client.UpdateContext.ChatId;
            var messageId = client.UpdateContext.MessageId;
            if (chatId == null || messageId == null)
                throw new MessageMayBeTooOld();

            await client.EditMessageTextAsync(
                chatId,
                messageId.Value,
                text,
                parseMode,
                entities,
                disableWebPagePreview,
                replyMarkup,
                cancellationToken);
        }

        public static async Task EditMessageTextAsync(
            this IAdvancedTelegramBotClient client,
            int messageId,
            string text,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity>? entities = default,
            bool? disableWebPagePreview = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default)
        {
            var chatId = client.UpdateContext.ChatId;
            if (chatId == null)
                throw new MessageMayBeTooOld();

            await client.EditMessageTextAsync(
                chatId,
                messageId,
                text,
                parseMode,
                entities,
                disableWebPagePreview,
                replyMarkup,
                cancellationToken);
        }
    }
}
