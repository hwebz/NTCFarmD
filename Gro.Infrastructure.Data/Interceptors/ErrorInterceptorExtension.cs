using log4net;
using System;

namespace Gro.Infrastructure.Data.Interceptors
{
    public static class ErrorInterceptorExtension
    {
        /// <summary>
        /// Add an error handling interceptor
        /// </summary>
        /// <param name="builder">RepositoryBuilder</param>
        /// <param name="log">Logger service</param>
        public static RepositoryBuilder<T> AddErrorHandler<T>(this RepositoryBuilder<T> builder, ILog log, Type[] thrownableExceptions)
            where T : class
        {
            var errorInterceptor = new ErrorInterceptor(log, thrownableExceptions);
            builder.AddInterceptor(errorInterceptor);
            return builder;
        }
    }
}
