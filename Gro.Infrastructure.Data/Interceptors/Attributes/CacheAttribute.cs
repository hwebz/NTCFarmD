using System;

namespace Gro.Infrastructure.Data.Interceptors.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CacheAttribute : Attribute
    {
        public string Key { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal class RefreshCacheParamAttribute : Attribute
    {
        public readonly string ParamName;
        public RefreshCacheParamAttribute(string paramName)
        {
            ParamName = paramName;
        }
    }
}
