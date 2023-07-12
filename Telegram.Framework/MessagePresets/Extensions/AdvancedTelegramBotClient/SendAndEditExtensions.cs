using Telegram.Framework.MessagePresets.Models;
using Telegram.Framework.TelegramBotApplication.AdvancedBotClient;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient
{
    public static class SendAndEditExtensions
    {
        public static async Task<Message> SendMessageResponseAsync(this IAdvancedTelegramBotClient client, MessagePreset preset)
        {
            return await client.SendMessageAsync(client.UpdateContext.ChatId, preset);
        }

        public static async Task<Message> SendMessageAsync(this IAdvancedTelegramBotClient client, ChatId chatId, MessagePreset preset)
        {
            return await client.SendTextMessageAsync(
                chatId,
                preset.Text,
                preset.MessageThreadId,
                preset.ParseMode,
                preset.Entities,
                preset.DisableWebPagePreview,
                preset.DisableNotification,
                preset.ProtectContent,
                preset.ReplyToMessageId,
                preset.AllowSendingWithoutReply,
                preset.ReplyMarkup,
                preset.CancellationToken
            );
        }

        public static async Task<Message> EditMessageResponseAsync(this IAdvancedTelegramBotClient client, int messageId, MessagePreset preset)
        {
            return await client.EditMessageAsync(client.UpdateContext.ChatId, messageId, preset);
        }

        public static async Task<Message> EditMessageAsync(this IAdvancedTelegramBotClient client, ChatId chatId, int messageId, MessagePreset preset)
        {
            if (preset.ReplyMarkup is not InlineKeyboardMarkup)
            {
                throw new ArgumentException("ReplyMarkup is not InlineKeyboardMarkup type");
            }

            return await client.EditMessageTextAsync(
                chatId,
                messageId,
                preset.Text,
                preset.ParseMode,
                preset.Entities,
                preset.DisableWebPagePreview,
                preset.ReplyMarkup as InlineKeyboardMarkup,
                preset.CancellationToken
            );
        }
    }
}
