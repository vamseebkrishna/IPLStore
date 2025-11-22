using IPLStore.Application.DTOs;
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

    private string? UserId => _userManager.GetUserId(User);

    public async Task<IActionResult> Index()
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = Url.Action("Index", "Cart") });

        var client = _factory.CreateClient("IPLApi");

        var response = await client.GetAsync($"api/cart/{UserId}");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return RedirectToAction("Login", "Account",
               new { ReturnUrl = Url.Action("Index", "Cart") });
        }

        if (!response.IsSuccessStatusCode)
            return View(new CartDto { UserId = UserId! });

        var cart = await response.Content.ReadFromJsonAsync<CartDto>()
                   ?? new CartDto { UserId = UserId! };

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account",
               new { ReturnUrl = Url.Action("Details", "Products", new { id = productId }) });
        }

        var client = _factory.CreateClient("IPLApi");

        await client.PostAsJsonAsync("api/cart/add", new
        {
            UserId = UserId,
            ProductId = productId,
            Quantity = quantity
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        if (!User.Identity!.IsAuthenticated)
            return RedirectToAction("Login", "Account");

        var client = _factory.CreateClient("IPLApi");

        await client.DeleteAsync($"api/cart/{UserId}");

        return RedirectToAction("Index");
    }
}
