using MassTransit;
using OnlineShop.OrderService.WebApi.Contracts;

namespace OnlineShop.OrderService.WebApi.FakeConsumers;

public class FakePaymentConsumer: IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        await context.Publish(new PaymentProcessed(context.Message.OrderId));
    }
}