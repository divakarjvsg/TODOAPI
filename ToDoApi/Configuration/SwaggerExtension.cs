using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace TodoAPI.Configuration
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// implements extension method for adding Swagger services.
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
               c.SwaggerDoc("v1", new OpenApiInfo
               {
                   Version = "v1",
                   Title = "TodoApi",
               });
               c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
               string filePath = Path.Combine(System.AppContext.BaseDirectory, "ToDoAPI.xml");
               c.IncludeXmlComments(filePath);
            });
        }
    }
}
