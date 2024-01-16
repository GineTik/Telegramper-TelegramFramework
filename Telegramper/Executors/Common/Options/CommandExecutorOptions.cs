using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.Common.Exceptions;

namespace Telegramper.Executors.Common.Options
{
    public class CommandExecutorOptions
    {
        /// default values see in <see cref="ExecutorOptions"/>
        
        private Type _nameTransformerNameTransformerType = null!;
        public Type NameTransformerType
        {
            get => _nameTransformerNameTransformerType;
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<INameTransformer>(value);
                _nameTransformerNameTransformerType = value;
            }
        }
    }
}
