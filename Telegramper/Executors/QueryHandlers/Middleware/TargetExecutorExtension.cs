using Telegramper.TelegramBotApplication;

namespace Telegramper.Executors.QueryHandlers.Middleware
{
    public static class TargetExecutorExtension
    {
        public static IBotApplication UseExecutors(this IBotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
