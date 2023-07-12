using Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Framework.TelegramBotApplication.AdvancedBotClient.Extensions
{
    public static class CallbackQueryExtensions
    {
        public static async Task DeleteMessageAsync(this IAdvancedTelegramBotClient client)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
            {
                throw new InvalidOperationException("UpdateType is not CallbackQuery");
            }

            await client.DeleteMessageAsync(
                client.UpdateContext.ChatId,
                client.UpdateContext.Update.CallbackQuery!.Message!.MessageId
            );
        }

        public static async Task DeleteMessageAsync(this IAdvancedTelegramBotClient client, int messageId)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
            {
                throw new InvalidOperationException("UpdateType is not CallbackQuery");
            }

            await client.DeleteMessageAsync(
                client.UpdateContext.ChatId,
                messageId
            );
        }

        public static async Task DeleteCallbackButtonAsync(this IAdvancedTelegramBotClient client,
            Message message, params string[] callbacksDatas)
        {
            await client.DeleteCallbackButtonAsync(client.UpdateContext.ChatId, message, callbacksDatas);
        }

        public static async Task DeleteCallbackButtonAsync(this IAdvancedTelegramBotClient client,
            ChatId chatId, Message message, params string[] callbacksDatas)
        {
            ArgumentNullException.ThrowIfNull(chatId);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(message.ReplyMarkup?.InlineKeyboard);

            var messageKeyboard = message.ReplyMarkup?.InlineKeyboard;
            var messageId = message.MessageId;
            var newKeyboard = messageKeyboard.Select(row =>
                row.SkipWhile(button => callbacksDatas.Contains(button.CallbackData))
            );

            await client.EditMessageReplyMarkupAsync(
                chatId,
                messageId,
                replyMarkup: new InlineKeyboardMarkup(newKeyboard)
            );
        }

        public static async Task DeleteCurrentCallbackButtonAsync(this IAdvancedTelegramBotClient client)
        {
            if (client.UpdateContext.Update.CallbackQuery?.Data is not { } data)
            {
                throw new InvalidOperationException("UpdateType is not CallbackQuery");
            }

            var message = client.UpdateContext.Update.CallbackQuery!.Message!;
            await client.DeleteCallbackButtonAsync(
                message,
                data
            );
        }

        public static async Task AnswerCallbackQueryAsync(this IAdvancedTelegramBotClient client)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidOperationException("UpdateType is not CallbackQuery");

            await client.AnswerCallbackQueryAsync(
               client.UpdateContext.Update.CallbackQuery!.Id
            );
        }
    }
}
