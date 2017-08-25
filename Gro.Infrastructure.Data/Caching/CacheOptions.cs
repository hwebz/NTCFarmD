using System;

namespace Gro.Infrastructure.Data.Caching
{
    public enum CacheStrategy
    {
        Sliding, Absolute, None
    }

    public class CacheOptions
    {
        public TimeSpan ExpirationTimeFromNow { get; set; }

        public TimeSpan SlidingExpirationTime { get; set; }

        public CacheStrategy Strategy { get; set; }

        public static bool RefreshCache = false;
    }
}
