using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorMethod
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<Attribute> _globalAttributes;

        public MethodInfo MethodInfo { get; }
        public Type ExecutorType =>
                    MethodInfo.DeclaringType ??
                    MethodInfo.ReflectedType ??
                    throw new InvalidOperationException($"Method {MethodInfo.Name} don't have DeclaringType and ReflectedType");
        public IEnumerable<TargetAttribute> TargetAttributes { get; } = default!;
        public IEnumerable<FilterAttribute> FilterAttributes { get; } = default!;

        public ExecutorMethod(MethodInfo methodInfo, IServiceProvider serviceProvider, IEnumerable<Attribute> globalAttributes)
        {
            MethodInfo = methodInfo;
            _serviceProvider = serviceProvider;
            _globalAttributes = globalAttributes;

            FilterAttributes = GetCustomAttributes<FilterAttribute>();
            TargetAttributes = GetCustomAttributes<TargetAttribute>();
            initializationTargetAttributes();
        }

        public IEnumerable<T> GetCustomAttributes<T>()
            where T : Attribute
        {
            return MethodInfo
                .GetCustomAttributes<T>()
                .Concat(_globalAttributes
                    .Where(attr => typeof(T).IsAssignableFrom(attr.GetType()))
                    .Cast<T>());
        }

        private void initializationTargetAttributes()
        {
            foreach (TargetAttribute targetAttribute in TargetAttributes)
            {
                targetAttribute.Initialization(this, _serviceProvider);
            }
        }
    }
}
