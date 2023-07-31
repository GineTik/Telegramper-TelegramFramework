using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Framework.Attributes.BaseAttributes;

namespace Telegram.Framework.Attributes.TargetExecutorAttributes
{
    [TargetUpdateTypes(UpdateType.Message)]
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
            var text = update.Message!.Text;
            if (text == null)
            {
                return false;
            }

            string command = takeCommandFromText(text);
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
