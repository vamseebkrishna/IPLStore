using IPLStore.Client.Models.Dto;
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

        string url = "api/products";

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

        var products = await client.GetFromJsonAsync<List<ProductListDto>>(url)
                       ?? new List<ProductListDto>();

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = _factory.CreateClient("IPLApi");

        var dto = await client.GetFromJsonAsync<ProductDetailsDto>($"api/products/{id}");

        if (dto == null)
            return NotFound();

        return View(dto);
    }
}
