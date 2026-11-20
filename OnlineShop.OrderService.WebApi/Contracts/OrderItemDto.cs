namespace OnlineShop.OrderService.WebApi.Contracts;

public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);