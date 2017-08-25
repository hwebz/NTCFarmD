using System;
using Castle.DynamicProxy;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Gro.Infrastructure.Data.Interceptors.Attributes;
using System.Collections;
using System.Collections.Generic;
using Gro.Infrastructure.Data.Caching;
using static Gro.Infrastructure.Data.Interceptors.MethodFormatter;

namespace Gro.Infrastructure.Data.Interceptors
{
    /// <summary>
    /// Castle Interceptor for custom caching
    /// </summary>
    public class CacheInterceptor : AsyncInterceptor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheOptions _cacheOptions;
        private readonly ILog _log;

        public CacheInterceptor(IMemoryCache memoryCache, ILog log, CacheOptions cacheOptions)
        {
            _memoryCache = memoryCache;
            _cacheOptions = cacheOptions;
            _log = log;
        }

        #region cache
        private static string GetDefaultMethodCacheKey(MethodInfo methodInfo, string cacheParamName, params object[] arguments)
        {
            if (arguments.Where(x => x != null).Any(x => (!x.GetType().IsValueType) && x.GetType() != typeof(string)))
            {
                return null;
            }

            var nameList = new List<string> { methodInfo.Name };
            var parameters = methodInfo.GetParameters();
            nameList.AddRange(arguments.Where(x => x != null).Where((t, i) => parameters[i].Name != cacheParamName).Select(t => t.ToString()));

            return string.Join("_", nameList);
        }

        private static string GetDefaulTaskCacheKey(MethodBase methodInfo, string cacheParamName, params object[] arguments)
        {
            var asyncIndex = methodInfo.Name.IndexOf("Async", StringComparison.OrdinalIgnoreCase);
            var methodName = methodInfo.Name.Substring(0, asyncIndex);

            var nameList = new List<string> { methodName };
            var parameters = methodInfo.GetParameters();
            nameList.AddRange(arguments.Where((t, i) => parameters[i].Name != cacheParamName).Select(t => t?.ToString()));

            return string.Join("_", nameList);
        }

        private bool TryGetCache<TValue>(string key, out TValue value)
        {
            object outValue;
            if (_memoryCache.TryGetValue(key, out outValue) && outValue is TValue)
            {
                //_log.Info($"Cache hit: {callerName}");
                value = (TValue)outValue;
                return true;
            }

            //_log.Info($"Cache miss: {callerName}");
            value = default(TValue);
            return false;
        }


        private void AddCache(string key, object value)
        {
            //do not cache null objects
            //reconsider this (ticket)
            if (value == null) return;

            //do not cache empty array/list
            if (value is IEnumerable)
            {
                var enumerable = (IEnumerable)value;

                if (!enumerable.GetEnumerator().MoveNext()) return;
            }

            _memoryCache.CreateOrSet(key, value, _cacheOptions);
        }

        private void RemoveCache(string key) => _memoryCache.Remove(key);

        #endregion

        private static bool GetRefreshCacheValue(IInvocation invocation, out string refreshParamName)
        {
            var cacheRefreshParamAttr = invocation.MethodInvocationTarget.GetCustomAttributes<RefreshCacheParamAttribute>().FirstOrDefault();
            var paramName = cacheRefreshParamAttr?.ParamName;
            if (string.IsNullOrWhiteSpace(paramName))
            {
                refreshParamName = null;
                return false;
            }

            var args = invocation.Arguments;
            var paramArray = invocation.Method.GetParameters();
            for (var i = 0; i < paramArray.Length; i++)
            {
                if (paramArray[i].Name == paramName)
                {
                    var value = args[i];
                    refreshParamName = paramName;
                    return (bool)value;
                }
            }

            refreshParamName = paramName;
            return false;
        }

        protected override void InterceptSync(IInvocation invocation)
        {
            var cacheAttribute = invocation.MethodInvocationTarget.GetCustomAttributes<CacheAttribute>().FirstOrDefault();
            if (cacheAttribute != null)
            {
                HandleReadMethod(invocation, cacheAttribute);
                return;
            }

            var cacheInvalidateAttributes = invocation.MethodInvocationTarget.GetCustomAttributes<CacheInvalidateAttribute>().ToArray();
            if (cacheInvalidateAttributes.Length > 0)
            {
                HandleWriteMethod(invocation, cacheInvalidateAttributes);
                return;
            }

            invocation.Proceed();
        }

        private void HandleWriteMethod(IInvocation invocation, IEnumerable<CacheInvalidateAttribute> invalidateAttributes)
        {
            foreach (var attr in invalidateAttributes)
            {
                var arguments = invocation.Arguments;
                var parameters = invocation.Method.GetParameters();
                var keyPattern = attr.Key;
                var cacheKey = FormatWithParameters(keyPattern, parameters, arguments);
                RemoveCache(cacheKey);
            }

            invocation.Proceed();
        }

        private void HandleReadMethod(IInvocation invocation, CacheAttribute cacheAttribute)
        {
            //find cache attribute
            string refreshParamName;
            var refreshCache = GetRefreshCacheValue(invocation, out refreshParamName);

            var arguments = invocation.Arguments;
            var parameters = invocation.Method.GetParameters();
            var keyPattern = cacheAttribute.Key;

            var cacheKey = string.IsNullOrWhiteSpace(keyPattern) ?
                GetDefaultMethodCacheKey(invocation.Method, refreshParamName, invocation.Arguments) : FormatWithParameters(keyPattern, parameters, arguments);

            object result;
            if (!refreshCache && TryGetCache(cacheKey, out result))
            {
                invocation.ReturnValue = result;
                return;
            }

            invocation.Proceed();
            AddCache(cacheKey, invocation.ReturnValue);
        }

        protected override void InterceptAsync<TResult>(IInvocation invocation)
        {
            //find cache attribute
            var cacheAttribute = invocation.MethodInvocationTarget.GetCustomAttributes<CacheAttribute>().FirstOrDefault();
            if (cacheAttribute != null)
            {
                HandleAsyncReadMethod<TResult>(invocation, cacheAttribute);
                return;
            }

            var cacheInvalidateAttributes = invocation.MethodInvocationTarget.GetCustomAttributes<CacheInvalidateAttribute>().ToArray();
            if (cacheInvalidateAttributes?.Length > 0)
            {
                HandleAsyncWriteMethod<TResult>(invocation, cacheInvalidateAttributes);
                return;
            }

            invocation.Proceed();
        }

        private void HandleAsyncReadMethod<TResult>(IInvocation invocation, CacheAttribute cacheAttribute)
        {
            //find cache attribute
            string refreshParamName;
            var refreshCache = GetRefreshCacheValue(invocation, out refreshParamName);

            var arguments = invocation.Arguments;
            var parameters = invocation.Method.GetParameters();
            var keyPattern = cacheAttribute.Key;

            var cacheKey = string.IsNullOrWhiteSpace(keyPattern) ?
                GetDefaulTaskCacheKey(invocation.Method, refreshParamName, invocation.Arguments) : FormatWithParameters(keyPattern, parameters, arguments);

            var result = default(TResult);

            if (!refreshCache && TryGetCache(cacheKey, out result))
            {
                var tcs = new TaskCompletionSource<TResult>();
                tcs.SetResult(result);
                invocation.ReturnValue = tcs.Task;
                return;
            }

            var returnTcs = new TaskCompletionSource<TResult>();

            invocation.Proceed();
            var invocationReturn = invocation.ReturnValue as Task<TResult>;
            invocationReturn?.ContinueWith(t =>
            {
                if (!t.IsFaulted && t.Status == TaskStatus.RanToCompletion)
                {
                    AddCache(cacheKey, t.Result);
                    result = t.Result;
                }
                else
                {
                    //TODO: add log here
                    if (t.Exception?.InnerException != null) throw t.Exception?.InnerException;
                }
                returnTcs.SetResult(result);
            });

            invocation.ReturnValue = returnTcs.Task;
        }

        private void HandleAsyncWriteMethod<TResult>(IInvocation invocation, CacheInvalidateAttribute[] invalidateAttributes)
            => HandleWriteMethod(invocation, invalidateAttributes);
    }
}
