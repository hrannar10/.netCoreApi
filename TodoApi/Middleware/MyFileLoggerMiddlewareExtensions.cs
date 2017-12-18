using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using TodoApi.Models;

namespace TodoApi.Middleware
{
    public static class MyFileLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyFileLogger(this IApplicationBuilder app, MyFileLoggerOptions options)
        {
            return app.UseMiddleware<MyFileLoggerMiddleware>(Options.Create(options));
        }
    }
}
