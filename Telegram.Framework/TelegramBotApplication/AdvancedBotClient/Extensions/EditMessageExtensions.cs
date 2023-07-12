using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions
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
            await client.EditMessageTextAsync(
                client.UpdateContext.ChatId,
                client.UpdateContext.MessageId,
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
            await client.EditMessageTextAsync(
                client.UpdateContext.ChatId,
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
