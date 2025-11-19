using System.Net.Http.Json;
using IPLStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.Client.Controllers;

public class CartController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string DemoUserId = "demo-user";

    public CartController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET /Cart
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var items = await client.GetFromJsonAsync<List<CartItem>>($"api/cart?userId={DemoUserId}")
                    ?? new List<CartItem>();

        return View(items);
    }

    // POST /Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var requestBody = new
        {
            UserId = DemoUserId,
            ProductId = productId,
            Quantity = quantity
        };

        var response = await client.PostAsJsonAsync("api/cart/add", requestBody);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    // POST /Cart/Checkout
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var requestBody = new { UserId = DemoUserId };

        var response = await client.PostAsJsonAsync("api/orders/create", requestBody);
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Checkout failed. Please ensure your cart is not empty.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index", "Orders");
    }
}
