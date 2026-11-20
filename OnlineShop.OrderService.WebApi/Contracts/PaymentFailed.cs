namespace OnlineShop.OrderService.WebApi.Contracts;

public record PaymentFailed(Guid OrderId, string Reason);