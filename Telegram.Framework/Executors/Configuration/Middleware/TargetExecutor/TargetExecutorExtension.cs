namespace Telegram.Framework.Executors.Configuration.Middleware.TargetExecutor
{
    public static class TargetExecutorExtension
    {
        public static TelegramBotApplication.BotApplication UseExecutors(this TelegramBotApplication.BotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
