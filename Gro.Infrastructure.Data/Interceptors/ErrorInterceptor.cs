using Castle.DynamicProxy;
using log4net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gro.Infrastructure.Data.Interceptors
{
    public class ErrorInterceptor : AsyncInterceptor
    {
        private readonly ILog _log;
        private readonly Type[] _thrownableExceptions;

        public ErrorInterceptor(ILog log, Type[] thrownableExceptions)
        {
            _log = log;
            _thrownableExceptions = thrownableExceptions;
        }

        protected override void InterceptAsync<TResult>(IInvocation invocation)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var isVoidTask = invocation.MethodInvocationTarget.ReturnType == typeof(Task);

            try
            {
                invocation.Proceed();
                var task = invocation.ReturnValue as Task;
                task?.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var exception = t.Exception?.InnerException ?? t.Exception;
                        if (exception != null && ShouldThrowException(exception))
                        {
                            tcs.SetException(exception);
                        }
                        else
                        {
                            var returnValue = HandleException(t.Exception?.InnerException, invocation, typeof(TResult));
                            tcs.SetResult(isVoidTask ? (TResult) new object() : (TResult) returnValue);
                        }
                    }
                    else if (t.Status == TaskStatus.RanToCompletion)
                    {
                        tcs.SetResult(isVoidTask ? (TResult) new object() : ((Task<TResult>) t).Result);
                    }
                });

                invocation.ReturnValue = tcs.Task;
            }
            catch (Exception ex)
            {
                if (ShouldThrowException(ex)) throw;

                var returnValue = HandleException(ex, invocation, typeof(TResult));
                tcs.SetResult((TResult) returnValue);
                invocation.ReturnValue = tcs.Task;
            }
        }

        protected override void InterceptSync(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                if (ShouldThrowException(ex)) throw;

                var returnValue = HandleException(ex, invocation, invocation.Method.ReturnType);
                if (invocation.Method.ReturnType != typeof(void))
                {
                    invocation.ReturnValue = returnValue;
                }
            }
        }

        private object HandleException(Exception exception, IInvocation invocation, Type returnType)
        {
            _log.Error($"Error when calling {invocation?.Method?.Name}: {exception?.Message}");
            return returnType != typeof(void) && returnType.IsValueType ? Activator.CreateInstance(returnType) : null;
        }

        private bool ShouldThrowException(Exception exception) => _thrownableExceptions.Any(e => e.IsInstanceOfType(exception));
    }
}