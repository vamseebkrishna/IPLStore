using IPLStore.Client.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace IPLStore.Client.Controllers;

public class CartController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(IHttpClientFactory factory, UserManager<IdentityUser> userManager)
    {
        _factory = factory;
        _userManager = userManager;
    }

    private string UserId => _userManager.GetUserId(User)!;

    public async Task<IActionResult> Index()
    {
        var client = _factory.CreateClient("IPLApi");

        // User not logged in → redirect immediately
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = Url.Action("Index", "Cart") });
        }

        var response = await client.GetAsync($"api/cart?userId={UserId}");

        // API returned 401 → redirect to login
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = Url.Action("Index", "Cart") });
        }

        // Other errors → show empty cart
        if (!response.IsSuccessStatusCode)
        {
            return View(new CartDto { UserId = UserId });
        }

        // Success → parse cart JSON
        var cart = await response.Content.ReadFromJsonAsync<CartDto>()
                   ?? new CartDto { UserId = UserId };

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity)
    {
        var client = _factory.CreateClient("IPLApi");

        var payload = new
        {
            UserId = UserId,
            ProductId = productId,
            Quantity = quantity
        };

        await client.PostAsJsonAsync("api/cart/add", payload);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        //var client = _factory.CreateClient("IPLApi");

        //var payload = new { UserId = UserId };

        //await client.PostAsJsonAsync("api/cart/clear", payload);

        //return RedirectToAction("Index");

        var client = _factory.CreateClient("IPLApi");
        var response = await client.DeleteAsync($"api/cart/{UserId}");

        return RedirectToAction("Index");
    }
}
