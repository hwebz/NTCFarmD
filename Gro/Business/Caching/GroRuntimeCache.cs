using System;
using System.Web;
using System.Web.Caching;
using Gro.Infrastructure.Data.Caching;

namespace Gro.Business.Caching
{
    public class GroRuntimeCache : IMemoryCache
    {
        private readonly Cache _runtimeCache = HttpRuntime.Cache;
        private readonly string _cacheName;

        public GroRuntimeCache(string name)
        {
            _cacheName = name;
        }

        public void CreateOrSet(string key, object value, CacheOptions cacheOption)
        {
            switch (cacheOption.Strategy)
            {
                case CacheStrategy.Absolute:
                    _runtimeCache.Insert($"{_cacheName}_{key}", value, null,
                        DateTime.Now.Add(cacheOption.ExpirationTimeFromNow), Cache.NoSlidingExpiration);
                    break;
                case CacheStrategy.Sliding:
                    _runtimeCache.Insert($"{_cacheName}_{key}", value, null, Cache.NoAbsoluteExpiration,
                        cacheOption.SlidingExpirationTime);
                    break;
                case CacheStrategy.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Remove(string key) => _runtimeCache.Remove($"{_cacheName}_{key}");

        public bool TryGetValue(string key, out object value)
        {
            var tryGetValue = _runtimeCache.Get($"{_cacheName}_{key}");
            if (tryGetValue == null)
            {
                value = null;
                return false;
            }

            value = tryGetValue;
            return true;
        }
    }
}
