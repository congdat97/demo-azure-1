using Application.Dto;

namespace GenericRepo_Dapper.Middlewares
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CategoryDtoProfile>();
                cfg.AddProfile<ProductDtoProfile>();
            });

            return services;
        }
    }
}
