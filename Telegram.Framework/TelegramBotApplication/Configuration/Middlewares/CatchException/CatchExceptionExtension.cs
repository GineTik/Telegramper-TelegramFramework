using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.TelegramBotApplication.Configuration.Middlewares.CatchException
{
    public static class CatchExceptionExtension
    {
        public static BotApplication UseCatchException(this BotApplication app, Action<UpdateContext, Exception> action)
        {
            return app.UseCatchException<Exception>(action);
        }

        public static BotApplication UseCatchException<TException>(this BotApplication app, Action<UpdateContext, TException> action)
            where TException : Exception
        {
            return app.Use(async (updateContext, next) =>
            {
                try
                {
                    await next();
                }
                catch (TException ex)
                {
                    action.Invoke(updateContext, ex);
                }
            });
        }

        public static BotApplication UseCatchException(this BotApplication app, Action<IServiceProvider, UpdateContext, Exception> action)
        {
            return app.UseCatchException<Exception>(action);
        }

        public static BotApplication UseCatchException<TException>(this BotApplication app, Action<IServiceProvider, UpdateContext, TException> action)
           where TException : Exception
        {
            return app.Use(async (provider, updateContext, next) =>
            {
                try
                {
                    await next();
                }
                catch (TException ex)
                {
                    action.Invoke(provider, updateContext, ex);
                }
            });
        }
    }
}
