using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using Telegramper.Core.AdvancedBotClient.Extensions;
using Telegramper.Core.Configuration.Middlewares;
using Telegramper.Core.Context;
using Telegramper.Core.Delegates;
using Telegramper.Executors.QueryHandlers.MethodInvoker;
using Telegramper.Executors.QueryHandlers.Preparer;
using Telegramper.Executors.QueryHandlers.SuitableMethodFinder;

namespace Telegramper.Executors.QueryHandlers.Middleware
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ISuitableMethodFinder _suitableMethodFinder;
        private readonly IExecutorMethodInvoker _executorMethodInvoker;
        private readonly IExecutorMethodPreparer _executorMethodPreparer;
        private readonly UpdateContext _updateContext;
        private readonly ILogger<ExecutorMethodInvoker> _logger;
        
        public TargetExecutorMiddleware(
            IExecutorMethodInvoker executorInvoker,
            ISuitableMethodFinder suitableMethodFinder,
            IExecutorMethodPreparer executorMethodPreparer,
            UpdateContextAccessor updateContextAccessor, ILogger<ExecutorMethodInvoker> logger)
        {
            _executorMethodInvoker = executorInvoker;
            _suitableMethodFinder = suitableMethodFinder;
            _executorMethodPreparer = executorMethodPreparer;
            _updateContext = updateContextAccessor.UpdateContext;
            _logger = logger;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var suitableRouteMethods = await _suitableMethodFinder.FindForCurrentUpdateAsync();
            var invokableMethods = _executorMethodPreparer.PrepareMethodsForExecution(suitableRouteMethods, out var errors).ToList();

            foreach (var error in errors)
            {
                await _updateContext.Client.SendTextMessageAsync(error.Message, parseMode: ParseMode.MarkdownV2);
            }

            if (invokableMethods.Any() == false)
            {
                _logger.LogDebug("No one handled the request");
                await next();
                return;
            }

            _logger.LogDebug($"The following methods will handle the request: {string.Join(", ", invokableMethods.Select(v => v.Method.MethodInfo.Name))}");
            await _executorMethodInvoker.InvokeAsync(invokableMethods);
        }
    }
}
