using System.Runtime.CompilerServices;

namespace chatApi.Setup
{
    public static class ApiKeyDependencyInjection
    {
        public static IServiceCollection AddApiToken (this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<ApiKeyConfiguration>(configuration.GetSection(key: "ApiKey"));
        }
        public static void UseApiTokenMiddleware(this WebApplication webApp)
        {
            webApp.UseWhen(predicate: context => !context.Request.Path.StartsWithSegments(other: "/health"),
                configuration:appBuilder => appBuilder.UseMiddleware<ApiKeyMiddleware>() 
            );
        }
    }
}
