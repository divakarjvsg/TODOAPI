using CorrelationId.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TodoAPI.Utilities;

namespace TodoAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, ICorrelationContextAccessor correlationContextAccessor)
        {
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
            _next = next;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;
            _logger.LogError($"Something went wrong: {exception}, Correlation id: {correlationId}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString());
        }
    }
}
