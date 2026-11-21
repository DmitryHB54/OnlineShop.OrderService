using MassTransit;
using OnlineShop.OrderService.WebApi.Contracts;

namespace OnlineShop.OrderService.WebApi.FakeConsumers;

public class FakeInventoryConsumer : IConsumer<ReserveStock>
{
    public async Task Consume(ConsumeContext<ReserveStock> context)
    {
        await context.Publish(new StockReserved(context.Message.OrderId));
    }
}