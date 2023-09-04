using Telegramper.Executors.QueryHandlers.Factory;
using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.TelegramBotApplication.Context;

namespace Telegramper.Executors.QueryHandlers.MethodInvoker
{
    public class ExecutorMethodInvoker : IExecutorMethodInvoker
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IExecutorFactory _executorFactory;
        private readonly UpdateContext _updateContext;

        public ExecutorMethodInvoker(
            IServiceProvider serviceProvider,
            IExecutorFactory executorFactory,
            UpdateContextAccessor updateContextAccessor)
        {
            _serviceProvider = serviceProvider;
            _executorFactory = executorFactory;
            _updateContext = updateContextAccessor.UpdateContext;
        }

        public async Task InvokeAsync(IEnumerable<InvokableExecutorMethod> invokableMethods)
        {
            var verifiedInvokableMethods = await beforeExecutionAsync(invokableMethods);

            foreach (var invokableMethod in verifiedInvokableMethods)
            {
                await invokableMethod.InvokeAsync(_executorFactory);
            }

            await afterExecutionAsync(verifiedInvokableMethods);
        }

        private async Task<IEnumerable<InvokableExecutorMethod>> beforeExecutionAsync(IEnumerable<InvokableExecutorMethod> invokableMethods)
        {
            var verifiedMethods = new List<InvokableExecutorMethod>();

            foreach (var invokableMethod in invokableMethods)
            {
                bool methodIsVerified = true;

                foreach (var filterAttribute in invokableMethod.Method.FilterAttributes)
                {
                    methodIsVerified = await filterAttribute.BeforeExecutionAsync(_serviceProvider, _updateContext);

                    if (methodIsVerified == false)
                    {
                        break;
                    }
                }

                if (methodIsVerified == true)
                {
                    verifiedMethods.Add(invokableMethod);
                }
            }

            return verifiedMethods;
        }

        private async Task afterExecutionAsync(IEnumerable<InvokableExecutorMethod> invokableMethods)
        {
            var filterAttributes = invokableMethods.SelectMany(invokableMethod => invokableMethod.Method.FilterAttributes);

            foreach (var filterAttribute in filterAttributes)
            {
                await filterAttribute.AfterExecutionAsync(_serviceProvider, _updateContext);
            }
        }
    }
}
