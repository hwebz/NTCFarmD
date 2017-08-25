using Castle.DynamicProxy;
using System.Collections.Generic;

namespace Gro.Infrastructure.Data
{
    public class RepositoryBuilder<TInterface> where TInterface : class
    {
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();
        private readonly TInterface _inner;

        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public RepositoryBuilder(TInterface implementation)
        {
            _inner = implementation;
        }

        public void AddInterceptor(IInterceptor interceptor) => _interceptors.Add(interceptor);

        public TInterface Build()
        {
            var proxy = Generator.CreateInterfaceProxyWithTarget(_inner, _interceptors.ToArray());
            return proxy;
        }
    }
}
