using Gro.Infrastructure.Data.Caching;
using log4net;

namespace Gro.Infrastructure.Data.Interceptors
{
    public static class CacheInterceptorExtension
    {
        /// <summary>
        /// Add a cache interptor to the service interceptor pipeline
        /// </summary>
        /// <param name="builder">RepositoryBuilder</param>
        /// <param name="cache">Cache service</param>
        /// <param name="log">Logger service</param>
        /// <param name="cacheOptions">Cache options</param>
        public static RepositoryBuilder<T> AddCache<T>(this RepositoryBuilder<T> builder, IMemoryCache cache, ILog log, CacheOptions cacheOptions)
            where T : class
        {
            var cacheInterceptor = new CacheInterceptor(cache, log, cacheOptions);
            builder.AddInterceptor(cacheInterceptor);
            return builder;
        }
    }
}
