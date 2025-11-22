using IPLStore.Application.Interfaces;
using IPLStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<CartDto>> GetCart(string userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest req)
    {
        await _cartService.AddToCartAsync(req.UserId, req.ProductId, req.Quantity);
        return Ok();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        await _cartService.ClearCartAsync(userId);
        return Ok();
    }
}

public record AddToCartRequest(string UserId, int ProductId, int Quantity);
