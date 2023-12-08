using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.QueryHandlers.Attributes.Targets
{
    [TargetUpdateType(UpdateType.Message)]
    public class TargetCommandAttribute : TargetAttribute
    {
        public string? Description { get; set; }
        public bool? Visible { get; set; }
        public string? Key { get; set; }
        
        private string? _command;

        public TargetCommandAttribute(string? command = null)
        {
            _command = command;
        }

        public override bool IsTarget(Update update)
        {
            var text = update.Message!.Text;
            
            if (text == null)
            {
                return false;
            }
            
            return _command == takeCommandFromText(text);
        }

        private static string takeCommandFromText(string text)
        {
            var command = text.Split(' ').First().TrimStart('/');
            command = Regex.Replace(command, "@\\w+", ""); // remove username
            return command;
        }

        protected override void Initialization(ExecutorMethod method)
        {
            _command ??= TransformedMethodName;
        }
    }
}
