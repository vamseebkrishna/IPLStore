using System.Net.Http.Json;
using IPLStore.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IPLStore.Client.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<IdentityUser> _userManager;

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
}
