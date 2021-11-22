using CorrelationId;
using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using TodoAPI.Graphql;
using TodoAPI.Queries;
using TodoAPI.Types;
using TodoAPI.Utilities;
using TodoAPI.Utilities.Handlers;
using ToDoApi.DataAccess.Repositories;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Context;

namespace TODOAPI
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
            services.AddHttpContextAccessor();
            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<TodoQuery>();
            services.AddScoped<TodoListTypes>();
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


            services.AddGraphQLServer()
                .AddType<TodoItemType>()
                .AddType<TodoListTypes>()
                .AddQueryType<TodoQuery>().AddMutationType<TodoMutation>();

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddSwaggerGen(c =>
            {                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TodoApi",
                });
            });
            services.AddControllers();
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
            app.UseAuthentication();
            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthorization();
            app.ConfigureExceptionMiddleware();
            //app.UseGraphQL.Playground();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGraphQL("/graphql");
            });
        }
    }
}
