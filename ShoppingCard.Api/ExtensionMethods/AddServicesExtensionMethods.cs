using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Repository;
using ShoppingCard.Repository.Implementations;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.ExtensionMethods
{
    public static class AddServicesExtensionMethods
    {
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("basketDb"));
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketProductRepository, BasketProductRepository>();
            services.AddScoped<ICachingService, CachingService>();
        }

    }
}
