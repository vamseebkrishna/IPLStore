using System.Net.Http.Json;
using IPLStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.Client.Controllers;

public class ProductsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET /Products?name=&type=&franchise=
    public async Task<IActionResult> Index(string? name, string? type, string? franchise)
    {
        var client = _httpClientFactory.CreateClient("IPLApi");

        string url = "api/products";

        // If any search parameters are provided, call the search endpoint
        if (!string.IsNullOrWhiteSpace(name) ||
            !string.IsNullOrWhiteSpace(type) ||
            !string.IsNullOrWhiteSpace(franchise))
        {
            var qs = new List<string>();
            if (!string.IsNullOrWhiteSpace(name))
                qs.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(type))
                qs.Add($"type={Uri.EscapeDataString(type)}");
            if (!string.IsNullOrWhiteSpace(franchise))
                qs.Add($"franchise={Uri.EscapeDataString(franchise)}");

            url = "api/products/search?" + string.Join("&", qs);
        }

        var products = await client.GetFromJsonAsync<List<Product>>(url) ?? new List<Product>();

        return View(products);
    }

    // GET /Products/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient("IPLApi");
        var product = await client.GetFromJsonAsync<Product>($"api/products/{id}");

        if (product == null)
            return NotFound();

        return View(product);
    }
}
