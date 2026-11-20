using Microsoft.EntityFrameworkCore;
using OnlineShop.OrderService.WebApi.Data;

namespace OnlineShop.OrderService.WebApi.Extensions;

public static class DbExtensions
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connecrtion string 'Default' is missing");

        services.AddDbContext<OrderStateDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}