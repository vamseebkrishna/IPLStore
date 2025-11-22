using IPLStore.Client.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace IPLStore.Client.Controllers;

public class OrdersController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(IHttpClientFactory factory, UserManager<IdentityUser> userManager)
    {
        _factory = factory;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    public async Task<IActionResult> Index()
    {
        var client = _factory.CreateClient("IPLApi");

        var orders = await client.GetFromJsonAsync<List<OrderDto>>(
            $"api/orders/user/{UserId}")
            ?? new List<OrderDto>();

        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        var client = _factory.CreateClient("IPLApi");

        var payload = new { UserId = UserId };

        var response = await client.PostAsJsonAsync("api/orders/create", payload);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }
}
