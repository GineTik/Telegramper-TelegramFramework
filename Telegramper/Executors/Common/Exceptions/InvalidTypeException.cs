namespace Telegramper.Executors.Common.Exceptions
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException(Type type) : base($"{type.Name} is invalid")
        {
        }

        public InvalidTypeException(string message) : base(message)
        {
        }

        public static void ThrowIfNotImplementation<T>(Type type)
        {
            if (type.IsInterface == true ||
                type.IsAbstract == true ||
                typeof(T).IsAssignableFrom(type) == false)
            {
                throw new InvalidTypeException($"Type {type.Name} is interface or abstract or not inherit {typeof(T).Name}");
            }
        }
        
        public static void ThrowIfNotInherit<T>(Type type)
        {
            if (type.IsInterface == true ||
                type.IsAbstract == true ||
                typeof(T).IsAssignableFrom(type) == false)
            {
                throw new InvalidTypeException($"Type {type.Name} is interface or abstract or not inherit {typeof(T).Name}");
            }
        }
    }
}
