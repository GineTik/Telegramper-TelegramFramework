using System.Reflection;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorMethod
    {
        private readonly IServiceProvider _serviceProvider;

        public MethodInfo MethodInfo { get; }
        public Type ExecutorType =>
                    MethodInfo.DeclaringType ??
                    MethodInfo.ReflectedType ??
                    throw new InvalidOperationException($"Method {MethodInfo.Name} don't have DeclaringType and ReflectedType");
        public IEnumerable<TargetAttribute> TargetAttributes { get; }
        public IEnumerable<FilterAttribute> FilterAttributes { get; }

        public ExecutorMethod(MethodInfo methodInfo, IServiceProvider serviceProvider, IEnumerable<Attribute> executorAttributes)
        {
            MethodInfo = methodInfo;
            _serviceProvider = serviceProvider;

            var attributes = executorAttributes.ToList();
            TargetAttributes = getCustomAttributes<TargetAttribute>(attributes);
            FilterAttributes = getCustomAttributes<FilterAttribute>(attributes);
            initializationTargetAttributes(TargetAttributes);
        }

        private IEnumerable<TAttribute> getCustomAttributes<TAttribute>(IEnumerable<Attribute> globalAttributes)
            where TAttribute : Attribute
        {
            return MethodInfo
                .GetCustomAttributes<TAttribute>()
                .Concat(globalAttributes
                    .Where(a => a is TAttribute)
                    .Cast<TAttribute>());
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
