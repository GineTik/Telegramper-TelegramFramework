using Telegramper.Executors.NameTransformer;
using Telegramper.Executors.Helpers.Exceptions;

namespace Telegramper.Executors.Configuration.Options
{
    public class CommandExecutorOptions
    {
        private Type _nameTransformerType = default!;
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
