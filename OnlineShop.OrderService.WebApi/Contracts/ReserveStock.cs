namespace OnlineShop.OrderService.WebApi.Contracts;

public record ReserveStock(Guid OrderId, List<OrderItemDto> Items);