using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Threading.Tasks;

namespace TodoAPI.Middleware
{
    public class ContentLocationMiddleware
    {
        private readonly RequestDelegate _next;

        public ContentLocationMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            loggerFactory.CreateLogger<ContentLocationMiddleware>();
            _next = next;
        }

        /// <summary>
        /// Process request.
        /// </summary>
        /// <returns>Returns nothing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                int responseStatusCode = context.Response.StatusCode;
                if (responseStatusCode == (int)HttpStatusCode.Created)
                {
                    IHeaderDictionary headers = context.Response.Headers;
                    StringValues locationHeaderValue = string.Empty;
                    if (headers.TryGetValue("Content-Location", out locationHeaderValue))
                    {
                        context.Response.Headers.Remove("Content-Location");
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                    else
                    {
                        context.Response.Headers.Add("Content-Location", context.Response.Headers["Location"]);
                    }
                }
                return Task.FromResult(0);
            });
            await _next(context);
        }
    }
    /// <summary>
    /// Extension of application builder for exception middleware.
    /// </summary>
    public static class ContentLocationMiddlewareExtensions
    {
        /// <summary>
        /// Configure Content-Location middleware.
        /// </summary>
        /// <param name="app">Application builder.</param>
        public static IApplicationBuilder UseContentLocationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ContentLocationMiddleware>();
        }

    }
}

