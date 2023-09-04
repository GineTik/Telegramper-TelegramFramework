using Telegram.Bot.Types.Enums;
using Telegramper.Executors.QueryHandlers.MethodInvoker;
using Telegramper.Executors.QueryHandlers.Preparer;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder;
using Telegramper.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegramper.TelegramBotApplication.Configuration.Middlewares;
using Telegramper.TelegramBotApplication.Context;
using Telegramper.TelegramBotApplication.Delegates;

namespace Telegramper.Executors.QueryHandlers.Middleware
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ISuitableMethodFinder _suitableMethodFinder;
        private readonly IExecutorMethodInvoker _executorMethodInvoker;
        private readonly IExecutorMethodPreparer _executorMethodPreparer;
        private readonly UpdateContext _updateContext;

        public TargetExecutorMiddleware(
            IExecutorMethodInvoker executorInvoker,
            ISuitableMethodFinder suitableMethodFinder,
            IExecutorMethodPreparer executorMethodPreparer,
            UpdateContextAccessor updateContextAccessor)
        {
            _executorMethodInvoker = executorInvoker;
            _suitableMethodFinder = suitableMethodFinder;
            _executorMethodPreparer = executorMethodPreparer;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var suitableMethods = await _suitableMethodFinder.FindForCurrentUpdateAsync();
            var invokableMethods = _executorMethodPreparer.PrepareMethodsForExecution(suitableMethods, out var errors);

            foreach (var error in errors)
            {
                await _updateContext.Client.SendTextMessageAsync(error.Message, parseMode: ParseMode.MarkdownV2);
            }

            if (invokableMethods.Any() == false)
            {
                await next();
                return;
            }

            await _executorMethodInvoker.InvokeAsync(invokableMethods);
        }
    }
}
