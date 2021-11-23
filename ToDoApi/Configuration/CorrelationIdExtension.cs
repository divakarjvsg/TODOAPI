using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using TodoAPI.Utilities.Handlers;

namespace TodoAPI.Configuration
{
    public static class CorrelationIdExtension
    {
        public static IServiceCollection AddCorrelationIdServices(this IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddTransient<NoOpDelegatingHandler>();
            services.AddHttpClient("ToDoApi")
                .AddCorrelationIdForwarding()
                .AddHttpMessageHandler<NoOpDelegatingHandler>();
            services.AddDefaultCorrelationId(options =>
             {
                 options.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
                 options.AddToLoggingScope = true;
                 options.EnforceHeader = false;
                 options.IgnoreRequestHeader = false;
                 options.IncludeInResponse = true;
                 options.RequestHeader = "Custom-Correlation-Id";
                 options.ResponseHeader = "X-Correlation-Id";
                 options.UpdateTraceIdentifier = false;
             });
            return services;
        }
    }
}
