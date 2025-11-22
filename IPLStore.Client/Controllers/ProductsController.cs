using IPLStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace IPLStore.Client.Controllers;

public class ProductsController : Controller
{
    private readonly IHttpClientFactory _factory;

    public ProductsController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<IActionResult> Index(string? name, string? type, string? franchise)
    {
        var client = _factory.CreateClient("IPLApi");

        // Build query string
        var query = $"api/products/search?name={name}&type={type}&franchise={franchise}";

        var products = await client.GetFromJsonAsync<IEnumerable<ProductDto>>(query);

        return View(products ?? Enumerable.Empty<ProductDto>());
    }


    public async Task<IActionResult> Details(int id)
    {
        var client = _factory.CreateClient("IPLApi");

        var product = await client.GetFromJsonAsync<ProductDetailsDto>($"api/products/details/{id}");

        if (product == null)
            return RedirectToAction("Index");

        return View(product);
    }
}
