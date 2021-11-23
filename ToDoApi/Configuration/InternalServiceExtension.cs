using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TodoAPI.Graphql;
using TodoAPI.Queries;
using ToDoApi.DataAccess.Repositories;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Context;

namespace TodoAPI.Configuration
{
    public static class InternalServiceExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<ITodoListsRepository, TodoListsRepository>();
            services.AddScoped<ITodoItemsRepository, TodoItemsRepository>();
            services.AddScoped<ILabelsRepository, LabelsRepository>();
            services.AddScoped<TodoQuery>();
            services.AddScoped<TodoMutation>();
            return services;
        }
    }
}
