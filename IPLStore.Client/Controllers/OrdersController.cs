using IPLStore.Application.DTOs;
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

    private string? UserId => _userManager.GetUserId(User);

    public async Task<IActionResult> Index()
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = Url.Action("Index", "Orders") });

        var client = _factory.CreateClient("IPLApi");

        var orders = await client.GetFromJsonAsync<IEnumerable<OrderDto>>(
            $"api/orders/{UserId}");

        return View(orders ?? Enumerable.Empty<OrderDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToAction("Login", "Account");

        var client = _factory.CreateClient("IPLApi");

        var response = await client.PostAsync($"api/orders/{UserId}", null);

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "Cart is empty.";

        return RedirectToAction("Index");
    }
}
