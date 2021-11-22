using Microsoft.AspNetCore.Builder;
using TodoAPI.Middleware;

namespace TodoAPI.Utilities
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
