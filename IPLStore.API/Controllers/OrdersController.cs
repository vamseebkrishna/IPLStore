using IPLStore.Application.Interfaces;
using IPLStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("{userId}")]
    public async Task<IActionResult> CreateOrder(string userId)
    {
        var orderId = await _orderService.CreateOrderAsync(userId);

        if (orderId == -1)
            return BadRequest("Cart is empty.");

        return Ok(orderId);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(string userId)
    {
        var orders = await _orderService.GetOrdersAsync(userId);
        return Ok(orders);
    }
}
