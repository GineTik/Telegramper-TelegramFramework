using Telegramper.Executors.Common.Models;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class ExecutorMethodStorageInitializer : IListStorageInitializer<ExecutorMethod>
    {
        private readonly IEnumerable<ExecutorType> _executorsTypes;
        private readonly IServiceProvider _serviceProvider;

        public ExecutorMethodStorageInitializer(
            IListStorage<ExecutorType> executorsTypes,
            IServiceProvider serviceProvider)
        {
            _executorsTypes = executorsTypes.Items;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ExecutorMethod> Initialization()
        {
            foreach (var executorTypeWrapper in _executorsTypes)
            {
                foreach (var methodInfo in executorTypeWrapper.Type.GetMethods())
                {
                    var executorMethod = new ExecutorMethod(methodInfo, _serviceProvider, executorTypeWrapper.Attributes);

                    if (executorMethod.TargetAttributes.Any())
                    {
                        yield return executorMethod;
                    }
                }
            }
        }
    }
}
