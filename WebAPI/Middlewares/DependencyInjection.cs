using Application.Logic.CategoryService;
using Application.Logic.ProductService;
using Application.Logic.UserService;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;

namespace GenericRepo_Dapper.Middlewares
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
