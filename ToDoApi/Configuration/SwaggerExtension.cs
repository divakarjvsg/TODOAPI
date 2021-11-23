using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TodoAPI.Configuration
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "TodoApi",
               });
           });
        }
    }
}
