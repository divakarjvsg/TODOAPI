using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoAPI.Context;
using TodoAPI.Repositories;
using TodoAPI.Interfaces;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TodoAPI.Utilities;
using GraphQL;
using TodoAPI.Queries;
using TodoAPI.Types;
using GraphQL.Types;
using TodoAPI.Schema;
using GraphiQl;

namespace TODOAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));


            services.AddScoped<ITodoListRepository, TodoListRepository>();
            services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddSingleton<IDocumentExecuter,DocumentExecuter>();
            services.AddScoped<TodoQuery>();
            services.AddScoped<TodoListTypes>();
            //var sp = services.BuildServiceProvider();
            //services.AddSingleton<ISchema>(new TodoSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\TodoApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TodoApi",
                });
            });
            #endregion
            services.AddControllers();
            
            services.AddLogging(config =>
            {
                config.AddDebug();
                //config.AddConsole();
                config.AddNLog();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<LogHeaderMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi");
                });
            }

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseGraphiQl();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
