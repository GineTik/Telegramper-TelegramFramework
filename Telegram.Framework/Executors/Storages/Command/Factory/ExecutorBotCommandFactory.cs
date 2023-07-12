using Telegram.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;

namespace Telegram.Framework.Executors.Storages.Command.Factory
{
    public class ExecutorBotCommandFactory : IBotCommandFactory
    {
        public IEnumerable<BotCommand> CreateBotCommands(MethodInfo method, TargetCommandsAttribute attribute)
        {
            yield return new BotCommand
            {
                Command = takeMainCommandFrom(attribute.Commands),
                Description = $"{takeHelpCommandsFrom(attribute.Commands)} {takeParametersFrom(method)} {attribute.Description}",
            };
        }

        private string takeMainCommandFrom(IEnumerable<string> commands)
        {
            return commands.First();
        }

        private string takeHelpCommandsFrom(IEnumerable<string> commands)
        {
            if (commands.Count() > 1)
                return "(" + string.Join(", ", commands.Skip(1).Select(c => "/" + c)) + ")";
            return "";
        }

        private string takeParametersFrom(MethodInfo method)
        {
            return "<" + string.Join("> <", method.GetParameters().Select(param => param.Name)) + ">";
        }
    }
}
