using MassTransit;
using Microsoft.EntityFrameworkCore;
using OnlineShop.OrderService.WebApi.Data;
using OnlineShop.OrderService.WebApi.FakeConsumers;
using OnlineShop.OrderService.WebApi.Saga;

namespace OnlineShop.OrderService.WebApi.Extensions;

public static class SagaExtensions
{
    public static IServiceCollection AddSaga(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' is missing");

        services.AddMassTransit(x =>
        {
            x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.UsePostgres();
                    r.AddDbContext<DbContext, OrderStateDbContext>((provider, options) =>
                    {
                        options.UseNpgsql(connectionString);
                    });
                });

            x.AddConsumer<FakeInventoryConsumer>();
            x.AddConsumer<FakePaymentConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("order-saga-queue", e =>
                {
                    e.ConfigureSaga<OrderState>(context);
                    e.UseInMemoryOutbox(context);
                });

                cfg.ConfigureEndpoints(context);
            });

        });
        return services;
    }
}