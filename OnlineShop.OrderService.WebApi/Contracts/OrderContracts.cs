namespace OnlineShop.OrderService.WebApi.Contracts;

// Command: Start order submit (comes from Controller)
public record SubmitOrder(Guid OrderId, Guid UserId, List<OrderItemDto> Items);
public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);

// Events from other services
public record StockReserved(Guid OrderId);
public record StockReservationFailed(Guid OrderId, string Reason);
public record PaymentProcessed(Guid OrderId);
public record PaymentFailed(Guid OrderId, string Reason);

// Commands to other services
public record ReserveStock(Guid OrderId, List<OrderItemDto> Items);
public record ProcessPayment(Guid OrderId, Guid UserId, decimal Amount);