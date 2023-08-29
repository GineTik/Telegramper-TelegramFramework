using Telegramper.TelegramBotApplication;

namespace Telegramper.Executors.Routing.Middleware.TargetExecutor
{
    public static class TargetExecutorExtension
    {
        public static IBotApplication UseExecutors(this IBotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
