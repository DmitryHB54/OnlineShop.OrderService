namespace OnlineShop.OrderService.WebApi.Contracts;

public record ProcessPayment(Guid OrderId, Guid UserId, decimal Amount);