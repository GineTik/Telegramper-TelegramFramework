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
        
        private string? _commandName;
        private string? _command;
        private string? _commandWithBotUserName;

        public TargetCommandAttribute(string? command = null)
        {
            _commandName = command;
        }
        
        protected override void Initialization(ExecutorMethod method)
        {
            _commandName ??= TransformedMethodName;
            _command = "/" + _commandName;
            _commandWithBotUserName = _command + "@" + Bot.Username;
        }

        public override bool IsTarget(Update update)
        {
            var text = update.Message!.Text;

            if (text == null)
            {
                return false;
            }

            var command = text
                .Split(" ")
                .First();

            return command.Equals(_command)
                   || command.Equals(_commandWithBotUserName);
        }
    }
}
