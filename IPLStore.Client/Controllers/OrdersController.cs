using System.Net.Http.Json;
using IPLStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.Client.Controllers;

public class OrdersController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string DemoUserId = "demo-user";

    public OrdersController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET /Orders
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        var orders = await client.GetFromJsonAsync<List<Order>>($"api/orders/user/{DemoUserId}")
                     ?? new List<Order>();

        return View(orders);
    }
}
