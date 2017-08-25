using System;

namespace Gro.Infrastructure.Data.Interceptors.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal class CacheInvalidateAttribute : Attribute
    {
        public readonly string Key;
        public CacheInvalidateAttribute(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
        }
    }
}
