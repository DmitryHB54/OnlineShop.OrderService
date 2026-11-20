using MassTransit;
using OnlineShop.OrderService.WebApi.Contracts;

namespace OnlineShop.OrderService.WebApi.Saga;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = null!;
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemDto> Items { get; set; } = null!;
    public DateTime CreateAt { get; set; }
    public string? FailureReason { get; set; }
}