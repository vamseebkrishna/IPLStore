using IPLStore.Client.Data;
using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace IPLStore.Client.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly AppDbContext _context;

    //public OrdersController(AppDbContext context)
    //{
    //    _context = context;
    //}


    public OrdersController(
        IHttpClientFactory httpClientFactory,
        UserManager<IdentityUser> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    // GET: /Orders
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var orders = await client.GetFromJsonAsync<List<Order>>(
            $"api/orders/user/{UserId}"
        ) ?? new List<Order>();

        return View(orders);
    }

    //[HttpPost("create")]
    //public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    //{
    //    // 1. Load cart items for the user
    //    var cartItems = await _context.CartItems
    //        .Where(ci => ci.UserId == request.UserId)
    //        .Include(ci => ci.Product)
    //        .ToListAsync();

    //    if (!cartItems.Any())
    //        return BadRequest("Cart is empty.");

    //    // 2. Create order
    //    var order = new Order
    //    {
    //        UserId = request.UserId,
    //        OrderDate = DateTime.UtcNow,
    //        TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price),
    //        Items = cartItems.Select(ci => new OrderItem
    //        {
    //            ProductId = ci.ProductId,
    //            Quantity = ci.Quantity,
    //            Price = ci.Product.Price
    //        }).ToList()
    //    };

    //    _context.Orders.Add(order);

    //    // 3. Clear cart
    //    _context.CartItems.RemoveRange(cartItems);

    //    await _context.SaveChangesAsync();

    //    return Ok(order);
    //}

    public class CreateOrderRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

}
