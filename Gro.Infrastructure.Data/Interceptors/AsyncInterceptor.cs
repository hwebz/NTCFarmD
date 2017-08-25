using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Data.Interceptors
{
    /// <summary>
    /// Interceptor capable of handling async methods
    /// </summary>
    public abstract class AsyncInterceptor : IInterceptor
    {
        [AttributeUsage(AttributeTargets.Method)]
        private class AsyncHandlerAttribute : Attribute
        {
        }

        [AsyncHandler]
        protected abstract void InterceptAsync<TResult>(IInvocation invocation);

        protected abstract void InterceptSync(IInvocation invocation);

        void IInterceptor.Intercept(IInvocation invocation)
        {
            var isAsync = typeof(Task).IsAssignableFrom(invocation.Method.ReturnType);

            if (!isAsync)
            {
                InterceptSync(invocation);
                return;
            }

            var handleAsyncMethod = GetType()
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Single(x => x.GetCustomAttributes<AsyncHandlerAttribute>().Any());

            var returnType = invocation.Method.ReturnType.IsGenericType ?
                invocation.Method.ReturnType.GetGenericArguments()[0] : typeof(object);

            var generic = handleAsyncMethod.MakeGenericMethod(returnType);
            generic.Invoke(this, new object[] { invocation });
        }
    }
}
