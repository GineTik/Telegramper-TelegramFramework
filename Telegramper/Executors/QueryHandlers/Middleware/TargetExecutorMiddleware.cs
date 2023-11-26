using Telegram.Bot.Types.Enums;
using Telegramper.Core.AdvancedBotClient.Extensions;
using Telegramper.Core.Configuration.Middlewares;
using Telegramper.Core.Context;
using Telegramper.Core.Delegates;
using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.MethodInvoker;
using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.Executors.QueryHandlers.Preparer;
using Telegramper.Executors.QueryHandlers.Preparer.PrepareErrors;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder;

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
            IEnumerable<ExecutorMethod> suitableMethods = await _suitableMethodFinder.FindForCurrentUpdateAsync();
            IEnumerable<InvokableExecutorMethod> invokableMethods = _executorMethodPreparer.PrepareMethodsForExecution(suitableMethods, out IEnumerable<Preparer.PrepareErrors.PrepareError>? errors);

            foreach (PrepareError error in errors)
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
