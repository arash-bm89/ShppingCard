using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Repository;
using ShoppingCard.Repository.Implementations;
using ShoppingCard.Service.IServices;
using ShoppingCard.Service.Services;

namespace ShoppingCard.Api.ExtensionMethods;

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
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<ICachedBasketService, CachedBasketService>();
        services.AddSingleton<IJwtService, JwtService>();
    }
}