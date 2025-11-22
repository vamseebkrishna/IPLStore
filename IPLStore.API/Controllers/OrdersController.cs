using IPLStore.API.Models;
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

    public record CreateOrderRequest(string UserId);

    // POST: api/orders/create
    [HttpPost("create")]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest req)
    {
        var cartItems = await _db.CartItems
            .Where(ci => ci.UserId == req.UserId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
            return BadRequest("Cart is empty.");

        var order = new Order
        {
            UserId = req.UserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product!.Price),
            Items = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product!.Price
            }).ToList()
        };

        _db.Orders.Add(order);
        _db.CartItems.RemoveRange(cartItems);

        await _db.SaveChangesAsync();

        var savedOrder = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstAsync(o => o.Id == order.Id);

        return Ok(ToDto(savedOrder));
    }

    // GET: api/orders/user/{id}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> History(string userId)
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .AsNoTracking()
            .ToListAsync();

        var result = orders.Select(ToDto).ToList();

        return Ok(result);
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var o = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);

        if (o == null)
            return NotFound();

        return Ok(ToDto(o));
    }

    private static OrderDto ToDto(Order o) =>
        new OrderDto
        {
            Id = o.Id,
            UserId = o.UserId,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "",
                FranchiseName = i.Product?.Franchise ?? "",
                Type = i.Product?.Type ?? "",
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        };
}
