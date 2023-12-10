using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegramper.Executors.Common.Options;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;
using Telegramper.Executors.QueryHandlers.Attributes.Supports;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorMethod
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<Attribute> _assemblyAttributes;

        public MethodInfo MethodInfo { get; }
        public Type ExecutorType =>
                    MethodInfo.DeclaringType ??
                    MethodInfo.ReflectedType ??
                    throw new InvalidOperationException($"Method {MethodInfo.Name} don't have DeclaringType and ReflectedType");
        public IEnumerable<TargetAttribute> TargetAttributes { get; }
        public IEnumerable<FilterAttribute> FilterAttributes { get; }

        public bool IsIgnoresLimitOfHandlers { get; }

        public ExecutorMethod(MethodInfo methodInfo, IServiceProvider serviceProvider, IEnumerable<Attribute> assemblyAttributes)
        {
            MethodInfo = methodInfo;
            _serviceProvider = serviceProvider;
            _assemblyAttributes = assemblyAttributes;

            // var attributes = assemblyAttributes.ToList();
            TargetAttributes = GetCustomAttributes<TargetAttribute>();
            FilterAttributes = GetCustomAttributes<FilterAttribute>();
            initializationTargetAttributes(TargetAttributes);

            IsIgnoresLimitOfHandlers = MethodInfo.GetCustomAttribute<IgnoreLimitOfHandlers>() != null;
        }

        public IEnumerable<TAttribute> GetCustomAttributes<TAttribute>()
            where TAttribute : Attribute
        {
            return MethodInfo.GetCustomAttributes<TAttribute>()
                .Concat(ExecutorType.GetCustomAttributes<TAttribute>())
                .Concat(_assemblyAttributes.Where(a => a is TAttribute).Cast<TAttribute>());
        }
        
        public TAttribute? GetCustomAttribute<TAttribute>()
            where TAttribute : Attribute
        {
            return MethodInfo.GetCustomAttribute<TAttribute>()
                   ?? ExecutorType.GetCustomAttribute<TAttribute>()
                   ?? _assemblyAttributes.FirstOrDefault(a => a is TAttribute) as TAttribute;
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
