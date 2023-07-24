using Telegram.Framework.TelegramBotApplication.Context;

namespace Telegram.Framework.TelegramBotApplication.Configuration.Middlewares.CatchException
{
    public static class CatchExceptionExtension
    {
        public static IBotApplication UseCatchException(this IBotApplication app, Action<UpdateContext, Exception> action)
        {
            return app.UseCatchException<Exception>(
                (provider, updateContext, ex) => action.Invoke(updateContext, ex)
            );
        }

        public static IBotApplication UseCatchException<TException>(this IBotApplication app, Action<UpdateContext, TException> action)
            where TException : Exception
        {
            return app.UseCatchException<TException>(
                (provider, updateContext, ex) => action.Invoke(updateContext, ex)
            );
        }

        public static IBotApplication UseCatchException(this IBotApplication app, Action<IServiceProvider, UpdateContext, Exception> action)
        {
            return app.UseCatchException<Exception>(action);
        }

        public static IBotApplication UseCatchException<TException>(this IBotApplication app, Action<IServiceProvider, UpdateContext, TException> action)
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
