using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
{
    [TargetUpdateType(UpdateType.Message)]
    public class TargetCommandAttribute : TargetAttribute
    {
        public string[] Commands { get; set; }
        public string? Description { get; set; }

        public TargetCommandAttribute(string? commands = null)
        {
            Commands = commands?.Replace(" ", "").Split(',')
                ?? new string[0];
        }

        public override bool IsTarget(Update update)
        {
            var text = update.Message!.Text;
            if (text == null)
            {
                return false;
            }

            string command = takeCommandFromText(text);
            if (Commands.Length == 0)
            {
                return command == TransformedMethodName;
            }

            return Commands.Contains(command);
        }

        private static string takeCommandFromText(string text)
        {
            var command = text.Split(' ').First().TrimStart('/');
            command = Regex.Replace(command, "@\\w+", ""); // remove username
            return command;
        }
    }
}
