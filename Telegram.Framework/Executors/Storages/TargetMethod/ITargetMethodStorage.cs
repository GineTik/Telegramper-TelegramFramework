using Telegram.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace Telegram.Framework.Executors.Storages.TargetMethod
{
    public interface ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext updateContext);
    }
}
