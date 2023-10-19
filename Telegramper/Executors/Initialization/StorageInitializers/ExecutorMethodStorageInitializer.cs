﻿using System.Reflection;
using Telegramper.Executors.Common.Models;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class ExecutorMethodStorageInitializer : IListStorageInitializer<ExecutorMethod>
    {
        private readonly IEnumerable<ExecutorTypeWrapper> _executorsTypes;
        private readonly IServiceProvider _serviceProvider;

        public ExecutorMethodStorageInitializer(
            IListStorage<ExecutorTypeWrapper> executorsTypes,
            IServiceProvider serviceProvider)
        {
            _executorsTypes = executorsTypes.Items;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ExecutorMethod> Initialization()
        {
            foreach (ExecutorTypeWrapper executorTypeWrapper in _executorsTypes)
            {
                foreach (MethodInfo methodInfo in executorTypeWrapper.Type.GetMethods())
                {
                    ExecutorMethod executorMethod = new ExecutorMethod(methodInfo, _serviceProvider, executorTypeWrapper.Attributes);

                    if (executorMethod.TargetAttributes.Any() == true)
                    {
                        yield return executorMethod;
                    }
                }
            }
        }
    }
}
