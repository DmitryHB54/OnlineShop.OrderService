using MassTransit;
using Microsoft.EntityFrameworkCore;
using OnlineShop.OrderService.WebApi.Contracts;

namespace OnlineShop.OrderService.WebApi.Saga;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State AwaitingStock { get; private set; } = null!;
    public State AwaitingPayment { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    public Event<SubmitOrder> SubmitOrderEvent { get; private set; } = null!;
    public Event<StockReserved> StockReservedEvent { get; private set; } = null!;
    public Event<StockReservationFailed> StockFailedEvent { get; private set; } = null!;
    public Event<PaymentProcessed> PaymentProcessedEvent { get; private set; } = null!;
    public Event<PaymentFailed> PaymentFailedEvent { get; private set; } = null!;

    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => SubmitOrderEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReservedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => StockFailedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentProcessedEvent, e => e.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentFailedEvent, e => e.CorrelateById(m => m.Message.OrderId));

        Initially(
            When(SubmitOrderEvent)
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.TotalAmount = context.Message.Items.Sum(i => i.UnitPrice * i.Quantity);
                    context.Saga.Items = context.Message.Items;
                    context.Saga.CreateAt = DateTime.UtcNow;
                })
                .Publish(context => new ReserveStock(context.Saga.CorrelationId, context.Saga.Items))
                .TransitionTo(AwaitingStock)
        );

        During(AwaitingStock,
            When(StockReservedEvent)
                .Publish(context => new ProcessPayment(context.Saga.CorrelationId, context.Saga.UserId, context.Saga.TotalAmount))
                .TransitionTo(AwaitingPayment),
            When(StockFailedEvent)
                .Then(context => context.Saga.FailureReason = context.Message.Reason)
                .TransitionTo(Failed)
                .Finalize()
        );

        During(AwaitingPayment,
            When(PaymentProcessedEvent)
                .TransitionTo(Completed),
            When(PaymentFailedEvent)
                .Then(context => context.Saga.FailureReason = context.Message.Reason)
                // compensation: ReleaseStock
                .TransitionTo(Failed)
                .Finalize()
        );
    }
}