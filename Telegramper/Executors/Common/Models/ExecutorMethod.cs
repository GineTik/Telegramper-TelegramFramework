using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegramper.Executors.Initialization.NameTransformer;
using Telegramper.Executors.QueryHandlers.Attributes.BaseAttributes;

namespace Telegramper.Executors.Common.Models
{
    public class ExecutorMethod
    {
        private readonly MethodInfo _methodInfo;
        private readonly IServiceProvider _serviceProvider;

        public ExecutorMethod(MethodInfo methodInfo, IServiceProvider serviceProvider)
        {
            _methodInfo = methodInfo;
            _serviceProvider = serviceProvider;
        }

        public MethodInfo MethodInfo => _methodInfo;

        public Type ExecutorType =>
                    _methodInfo.DeclaringType ??
                    _methodInfo.ReflectedType ??
                    throw new InvalidOperationException($"Method {_methodInfo.Name} don't have DeclaringType and ReflectedType");

        private IEnumerable<TargetAttribute> _targetAttributes = default!;
        public IEnumerable<TargetAttribute> TargetAttributes
        {
            get
            {
                if (_targetAttributes == null)
                {
                    _targetAttributes = _methodInfo.GetCustomAttributes<TargetAttribute>();

                    foreach (var targetAttribute in _targetAttributes)
                    {
                        targetAttribute.Initialization(this, _serviceProvider);
                    }
                }

                return _targetAttributes;
            }
        }

        private IEnumerable<FilterAttribute> _filterAttributes = default!;
        public IEnumerable<FilterAttribute> FilterAttributes =>
            _filterAttributes ??= _methodInfo.GetCustomAttributes<FilterAttribute>();
    }
}
