﻿using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Models;
using Telegramper.Executors.QueryHandlers.Preparer.Models;

namespace Telegramper.Executors.QueryHandlers.Preparer
{
    public interface IExecutorMethodPreparer
    {
        IEnumerable<InvokableExecutorMethod> PrepareMethodsForExecution(IEnumerable<Route> routes, out IEnumerable<PrepareError> prepareErrors);
    }
}
