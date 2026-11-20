namespace OnlineShop.OrderService.WebApi.Contracts;

public record StockReservationFailed(Guid OrderId, string Reason);