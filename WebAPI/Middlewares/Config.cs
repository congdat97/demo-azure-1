using Microsoft.Extensions.Options;
using Utility.Model;

namespace GenericRepo_Dapper.Middlewares
{
    public static class Config
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            services.Configure<ConnectDatabase>(configuration.GetSection("ConnectionStrings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<ConnectDatabase>>().Value);

            return services;
        }
    }
}
