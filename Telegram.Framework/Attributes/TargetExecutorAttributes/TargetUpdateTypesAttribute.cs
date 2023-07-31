﻿using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Framework.Attributes.BaseAttributes;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TargetUpdateTypesAttribute : TargetAttribute
    {
        public UpdateType[] UpdateTypes { get; set; }

        public TargetUpdateTypesAttribute(params UpdateType[] updateTypes)
        {
            UpdateTypes = updateTypes;
        }

        public override bool IsTarget(Update update)
        {
            return UpdateTypes.Contains(update.Type);
        }
    }
}
