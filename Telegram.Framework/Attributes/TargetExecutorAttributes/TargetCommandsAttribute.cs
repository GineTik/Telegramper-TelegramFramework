using Telegram.Framework.Attributes.BaseAttributes;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
{
    [TargetUpdateType(UpdateType.Message)]
    public class TargetCommandsAttribute : TargetAttribute
    {
        public string[] Commands { get; set; }
        public string? Description { get; set; }

        public TargetCommandsAttribute(string commands)
        {
            Commands = commands.Replace(" ", "").Split(',');
        }

        public override bool IsTarget(Update update)
        {
            if (update.Message!.Text is not { } text)
                return false;

            var command = text.Split(' ').First().TrimStart('/');
            command = Regex.Replace(command, "@\\w+", ""); // remove bot username

            return Commands.Contains(command);
        }
    }
}
