﻿using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegramper.Core.AdvancedBotClient.Extensions;
using Telegramper.Core.Context;

namespace Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes
{
    public abstract class ValidationAttribute : FilterAttribute
    {
        public string? ErrorMessage { get; set; } = default!;
        public ParseMode ParseMode { get; set; } = ParseMode.MarkdownV2;

        public abstract Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider);

        public override async Task<bool> BeforeExecutionAsync(IServiceProvider serviceProvider, UpdateContext updateContext)
        {
            var result = await ValidateAsync(updateContext, serviceProvider);

            if (result == false && string.IsNullOrEmpty(ErrorMessage) == false)
            {
                await updateContext.Client.SendTextMessageAsync(ErrorMessage, parseMode: ParseMode);
            }

            return result;
        }
    }
}
