using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.OrderService.WebApi.Contracts;

namespace OnlineShop.OrderService.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder([FromBody] List<OrderItemDto> items)
    {
        var orderId = Guid.NewGuid();
        var userId = Guid.Parse(User.FindFirst("userId")!.Value);

        await _publishEndpoint.Publish(new SubmitOrder(orderId, userId, items));

        return Accepted(new { OrderId = orderId });
    }
}