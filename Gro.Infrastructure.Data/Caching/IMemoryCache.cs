namespace Gro.Infrastructure.Data.Caching
{
    public interface IMemoryCache
    {
        /// <summary>
        /// Try to get the cached value based on key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache value</param>
        /// <returns>Whether the key exists</returns>
        bool TryGetValue(string key, out object value);

        /// <summary>
        /// Add or set a cache value
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache value</param>
        /// <param name="cacheOptions">Cache options</param>
        void CreateOrSet(string key, object value, CacheOptions cacheOptions);

        /// <summary>
        /// Remove the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        void Remove(string key);
    }
}
