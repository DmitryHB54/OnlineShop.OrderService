namespace OnlineShop.OrderService.WebApi.Contracts;

public record SubmitOrder(Guid OrderId, Guid UserId, List<OrderItemDto> Items);