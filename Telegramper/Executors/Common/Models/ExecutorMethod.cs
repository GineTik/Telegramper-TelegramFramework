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
        public IEnumerable<TargetAttribute> TargetAttributes { get; }
        public IEnumerable<FilterAttribute> FilterAttributes { get; }

        public ExecutorMethod(MethodInfo methodInfo, IServiceProvider serviceProvider, IEnumerable<Attribute> globalAttributes)
        {
            MethodInfo = methodInfo;
            _serviceProvider = serviceProvider;
            _globalAttributes = globalAttributes;

            FilterAttributes = getCustomAttributes<FilterAttribute>();
            TargetAttributes = getCustomAttributes<TargetAttribute>();
            initializationTargetAttributes(TargetAttributes);
        }

        private IEnumerable<T> getCustomAttributes<T>()
            where T : Attribute
        {
            return MethodInfo
                .GetCustomAttributes<T>()
                .Concat(_globalAttributes
                    .Where(attr => typeof(T).IsAssignableFrom(attr.GetType()))
                    .Cast<T>());
        }

        private void initializationTargetAttributes(IEnumerable<TargetAttribute> targetAttributes)
        {
            foreach (var targetAttribute in targetAttributes)
            {
                targetAttribute.Initialization(this, _serviceProvider);
            }
        }
    }
}
