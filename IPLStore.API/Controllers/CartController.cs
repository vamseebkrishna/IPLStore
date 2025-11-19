using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _db;

    public CartController(AppDbContext db)
    {
        _db = db;
    }

    // DTO for add-to-cart request
    public record AddToCartRequest(string UserId, int ProductId, int Quantity);

    // POST: api/cart/add
    // Add product to cart
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        if (request.Quantity <= 0)
            return BadRequest("Quantity must be at least 1.");

        var product = await _db.Products.FindAsync(request.ProductId);
        if (product == null)
            return NotFound("Product not found.");

        var existing = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == request.UserId && ci.ProductId == request.ProductId);

        if (existing == null)
        {
            var cartItem = new CartItem
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            _db.CartItems.Add(cartItem);
        }
        else
        {
            existing.Quantity += request.Quantity;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    // GET: api/cart?userId=demo-user
    // Get cart items for a user
    [HttpGet]
    public async Task<IActionResult> GetCart([FromQuery] string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("UserId is required.");

        var items = await _db.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        return Ok(items);
    }

    // POST: api/cart/clear
    // Clear the cart for a user
    public record ClearCartRequest(string UserId);

    [HttpPost("clear")]
    public async Task<IActionResult> ClearCart([FromBody] ClearCartRequest request)
    {
        var items = _db.CartItems.Where(ci => ci.UserId == request.UserId);
        _db.CartItems.RemoveRange(items);

        await _db.SaveChangesAsync();
        return Ok();
    }
}
