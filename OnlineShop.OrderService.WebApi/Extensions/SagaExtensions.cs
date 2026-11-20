using MassTransit;
using Microsoft.EntityFrameworkCore;
using OnlineShop.OrderService.WebApi.Data;
using OnlineShop.OrderService.WebApi.Saga;

namespace OnlineShop.OrderService.WebApi.Extensions;

public static class SagaExtensions
{
    public static IServiceCollection AddSaga(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connecrtion string 'Default' is missing");

        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.AddDbContext<DbContext, OrderStateDbContext>((provider, options) =>
                    {
                        options.UseNpgsql(connectionString);
                    });
                });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}