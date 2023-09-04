using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.Common.Exceptions;

namespace Telegramper.Executors.Common.Options
{
    public class CommandExecutorOptions
    {
        private Type _nameTransformerType = typeof(SnakeCaseNameTransformer);
        public Type Type
        {
            get
            {
                return _nameTransformerType;
            }
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<INameTransformer>(value);
                _nameTransformerType = value;
            }
        }
    }
}
