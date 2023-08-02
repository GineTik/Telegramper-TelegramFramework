using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegramper.TelegramBotApplication.Exceptions;

namespace Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions
{
    public static class CallbackQueryExtensions
    {
        public static async Task DeleteMessageAsync(this IAdvancedTelegramBotClient client)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidUpdateTypeException("UpdateType is not CallbackQuery");

            var chatId = client.UpdateContext.ChatId;
            var messageId = client.UpdateContext.MessageId;
            if (chatId == null || messageId == null)
                throw new MessageMayBeTooOld();

            await client.DeleteMessageAsync(
                chatId.Value,
                messageId.Value
            );
        }

        public static async Task DeleteMessageAsync(this IAdvancedTelegramBotClient client, int messageId)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidUpdateTypeException("UpdateType is not CallbackQuery");

            var chatId = client.UpdateContext.ChatId;
            if (chatId == null)
                throw new MessageMayBeTooOld();

            await client.DeleteMessageAsync(
                chatId,
                messageId
            );
        }

        public static async Task DeleteCallbackButtonAsync(this IAdvancedTelegramBotClient client,
            Message message, params string[] callbacksDatas)
        {
            var chatId = client.UpdateContext.ChatId;
            if (chatId == null)
                throw new MessageMayBeTooOld();

            await client.DeleteCallbackButtonAsync(chatId, message, callbacksDatas);
        }

        public static async Task DeleteCallbackButtonAsync(this IAdvancedTelegramBotClient client,
            ChatId chatId, Message message, params string[] callbacksDatas)
        {
            ArgumentNullException.ThrowIfNull(chatId);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(message.ReplyMarkup?.InlineKeyboard);

            var messageKeyboard = message.ReplyMarkup?.InlineKeyboard;
            var messageId = message.MessageId;
            
            var newKeyboard = messageKeyboard?.Select(row =>
                row.SkipWhile(button => callbacksDatas.Contains(button.CallbackData))
            );
            var replyMarkup = newKeyboard == null ? null : new InlineKeyboardMarkup(newKeyboard);

            await client.EditMessageReplyMarkupAsync(
                chatId,
                messageId,
                replyMarkup: replyMarkup
            );
        }

        public static async Task DeleteCurrentCallbackButtonAsync(this IAdvancedTelegramBotClient client)
        {
            if (client.UpdateContext.Update.CallbackQuery?.Data is not { } data)
                throw new InvalidUpdateTypeException("UpdateType is not CallbackQuery");

            var message = client.UpdateContext.Message;
            if (message == null)
                throw new MessageMayBeTooOld();

            await client.DeleteCallbackButtonAsync(
                message,
                data
            );
        }

        public static async Task AnswerCallbackQueryAsync(
            this IAdvancedTelegramBotClient client,
            string? text = null,
            bool? showAlert = null,
            string? url = null,
            int? cacheTime = null,
            CancellationToken cancellationToken = default)
        {
            if (client.UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidUpdateTypeException("UpdateType is not CallbackQuery");

            await client.AnswerCallbackQueryAsync(
               client.UpdateContext.Update.CallbackQuery!.Id,
               text,
               showAlert,
               url,
               cacheTime,
               cancellationToken
            );
        }
    }
}
