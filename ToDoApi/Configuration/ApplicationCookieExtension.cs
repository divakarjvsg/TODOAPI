using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TodoAPI.Configuration
{
    public static class ApplicationCookieExtension
    {
        public static IServiceCollection AddApplicationCookie(this IServiceCollection services)
        {
            return services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
        }
    }
}
