using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;
using TodoAPI.Configuration;
using TodoAPI.Middleware;
using TodoAPI.Utilities;
using ToDoApi.Database.Context;

namespace ToDoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddInternalServices();
            services.AddCorrelationIdServices();
            services.AddGraphQLServices();
            services.AddApplicationCookie();
            services.AddSwagger();
            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            })
                    .AddNewtonsoftJson();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            ServiceProvider builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<LogHeaderMiddleware>();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi");
                });                
            }
            app.UseCorrelationId();
            app.UseContentLocationMiddleware();
            app.UseAuthentication();
            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthorization();
            app.ConfigureExceptionMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGraphQL();
            });
        }
    }
}
