using Microsoft.AspNetCore.Builder;
using MSToolKit.Filters.Options;
using System;

namespace MSToolKit.Filters.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingExceptionHandler(
            this IApplicationBuilder app, Action<LoggingExceptionHandlerOptions> exceptionOptions)
        {
            var options = new LoggingExceptionHandlerOptions();
            exceptionOptions?.Invoke(options);

            app.UseMiddleware<LoggingExceptionHandler>(options);

            return app;
        }
    }
}
