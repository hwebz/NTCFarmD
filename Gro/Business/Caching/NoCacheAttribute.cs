using System;
using System.Web;
using System.Web.Mvc;

namespace Gro.Business.Caching
{
    public class NoCacheAttribute : FilterAttribute, IResultFilter
    {
        private static DateTime Expires = DateTime.Now.AddYears(-5);

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetRevalidation(HttpCacheRevalidation.ProxyCaches);
            cache.SetExpires(Expires);
            cache.AppendCacheExtension("private");
            cache.AppendCacheExtension("no-cache=Set-Cookie");
            cache.SetProxyMaxAge(TimeSpan.Zero);
        }
    }
}
