﻿using Microsoft.Extensions.Logging;
using Telegramper.Core.Configuration.Middlewares;
using Telegramper.Core.Context;
using Telegramper.Core.Delegates;

namespace Telegramper.Core
{
    public interface IBotApplication
    {
        IServiceProvider Services { get; }
        ILogger Logger { get; }

        IBotApplication Use(Func<UpdateContext, NextDelegate, Task> middlware);
        IBotApplication Use(Func<IServiceProvider, UpdateContext, NextDelegate, Task> middlware);
        IBotApplication UseMiddleware<T>() where T : class, IMiddleware;
        void RunPolling();
    }
}
