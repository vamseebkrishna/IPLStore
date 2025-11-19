using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }

    // POST: api/orders/create
    // Create an order from the user's cart
    public record CreateOrderRequest(string UserId);

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var cartItems = await _db.CartItems
            .Where(ci => ci.UserId == request.UserId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
            return BadRequest("Cart is empty.");

        var total = cartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity);

        var order = new Order
        {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = total,
            Items = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product!.Price
            }).ToList()
        };

        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cartItems);

        await _db.SaveChangesAsync();

        // return created order id
        return Ok(new { orderId = order.Id });
    }

    // GET: api/orders/user/demo-user
    // Get order history for a user
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersForUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return Ok(orders);
    }
}
