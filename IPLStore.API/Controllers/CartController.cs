using IPLStore.API.Models;
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

    // Request DTOs
    public record AddToCartRequest(string UserId, int ProductId, int Quantity);
    public record ClearCartRequest(string UserId);

    // POST: api/cart/add
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest req)
    {
        if (req.Quantity <= 0)
            return BadRequest("Quantity must be at least 1.");

        var product = await _db.Products.FindAsync(req.ProductId);
        if (product == null)
            return NotFound("Product not found.");

        var existing = await _db.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == req.UserId && ci.ProductId == req.ProductId);

        if (existing == null)
        {
            _db.CartItems.Add(new CartItem
            {
                UserId = req.UserId,
                ProductId = req.ProductId,
                Quantity = req.Quantity
            });
        }
        else
        {
            existing.Quantity += req.Quantity;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    // GET: api/cart?userId=abc
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart([FromQuery] string userId)
    {
        var items = await _db.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .AsNoTracking()
            .ToListAsync();

        var dto = new CartDto
        {
            UserId = userId,
            Items = items.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? "",
                FranchiseName = ci.Product?.Franchise ?? "",
                Type = ci.Product?.Type ?? "",
                UnitPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity
            }).ToList()
        };

        return Ok(dto);
    }

    // POST: api/cart/clear
    [HttpDelete("{userId}")]
    public async Task<IActionResult> Clear(string userId)
    {
        var items = _db.CartItems.Where(ci => ci.UserId == userId);
        _db.CartItems.RemoveRange(items);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
