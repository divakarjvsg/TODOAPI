using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using TodoAPI.Graphql;
using TodoAPI.Queries;
using TodoAPI.Types;
using TodoAPI.Utilities;
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

            //services.AddGraphQL(s => SchemaBuilder.New()
            //    .AddServices(s)
            //    .AddType<TodoItemType>()
            //    .AddType<TodoListTypes>()
            //    .AddQueryType<TodoQuery>()
            //    .AddMutationType<TodoMutation>()                
            //    .Create());

            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddGraphQLServer()
                .AddType<TodoItemType>()
                .AddType<TodoListTypes>()
                .AddQueryType<TodoQuery>().AddMutationType<TodoMutation>();


            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


            //services.AddScoped<TodoSchema>();
            //var sp = services.BuildServiceProvider();
            //services.AddSingleton<ISchema>(new TodoSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            //services.AddGraphQL(options =>
            //{
            //    options.EnableMetrics = true;
            //})
            //.AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
            //.AddSystemTextJson()
            //.AddUserContextBuilder(httpContext => new GraphQLUserContext { User = httpContext.User });

            //services.AddDirectoryBrowser();
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\TodoApi.xml", System.AppDomain.CurrentDomain.BaseDirectory));
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
                //app.UseFileServer(new FileServerOptions
                //{
                //    FileProvider = new PhysicalFileProvider(
                //       System.IO.Path.Combine(Directory.GetCurrentDirectory(), "swaggerschema/v1")),
                //    RequestPath = "/swagger/v1",
                //    EnableDirectoryBrowsing = true
                //});

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi");
                });
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseAuthorization();
            //app.UseGraphQL();
            //app.UseGraphQLPlayground();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGraphQL("/graphql");
            });
        }
    }
}
