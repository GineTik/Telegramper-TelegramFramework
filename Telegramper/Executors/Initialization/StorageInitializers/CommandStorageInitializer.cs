using Telegramper.Executors.Common.Models;
using Telegramper.Executors.QueryHandlers.Attributes.Targets;
using Telegramper.Storage.Initializers;
using Telegramper.Storage.List;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class CommandStorageInitializer : IListStorageInitializer<TargetCommandAttribute>
    {
        private readonly IEnumerable<ExecutorMethod> _executorsMethods;

        public CommandStorageInitializer(IListStorage<ExecutorMethod> executorsMethods)
        {
            _executorsMethods = executorsMethods.Items;
        }

        public IEnumerable<TargetCommandAttribute> Initialization()
        {
            return _executorsMethods
                .SelectMany(method => method
                    .TargetAttributes
                    .Where(attr => attr
                        .GetType().Name == nameof(TargetCommandAttribute)
                    )
                ).Cast<TargetCommandAttribute>();
        }
    }
}
