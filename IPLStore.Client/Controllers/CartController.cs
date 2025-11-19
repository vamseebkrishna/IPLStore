using System.Net.Http.Json;
using IPLStore.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IPLStore.Client.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(
        IHttpClientFactory httpClientFactory,
        UserManager<IdentityUser> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    // GET: /Cart
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var items = await client.GetFromJsonAsync<List<CartItem>>(
            $"api/cart?userId={UserId}"
        ) ?? new List<CartItem>();

        return View(items);
    }

    // POST: /Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var requestBody = new
        {
            UserId = UserId,
            ProductId = productId,
            Quantity = quantity
        };

        var response = await client.PostAsJsonAsync("api/cart/add", requestBody);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    // POST: /Cart/Checkout
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var requestBody = new { UserId = UserId };

        var response = await client.PostAsJsonAsync("api/orders/create", requestBody);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Checkout failed. Please ensure your cart is not empty.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index", "Orders");
    }
}
