using Telegram.Framework.TelegramBotApplication;

namespace Telegram.Framework.Executors.Configuration.Middleware.TargetExecutor
{
    public static class TargetExecutorExtension
    {
        public static IBotApplication UseExecutors(this IBotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
