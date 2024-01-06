using Telegramper.Executors.Common.Models;
using Telegramper.Storage.Initializers;

namespace Telegramper.Executors.Initialization.StorageInitializers
{
    public class ExecutorTypeStorageInitializer : IListStorageInitializer<ExecutorType>
    {
        private readonly IEnumerable<SmartAssembly> _assemblies;

        public ExecutorTypeStorageInitializer(IEnumerable<SmartAssembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public IEnumerable<ExecutorType> Initialization()
        {
            return _assemblies.SelectMany(smartAssembly => smartAssembly
                .Assembly
                .GetTypes()
                .Where(executorType => executorType != typeof(Executor)
                       && typeof(Executor).IsAssignableFrom(executorType))
                .Select(type => new ExecutorType(type, smartAssembly.AssemblyAttributes)));
        }
    }
}
