using Telegramper.Executors.Building.NameTransformer;
using Telegramper.Executors.Building.Exceptions;

namespace Telegramper.Executors.Build.Options
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
