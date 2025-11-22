using IPLStore.Application.Interfaces;
using IPLStore.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    //[HttpGet("{id:int}")]
    //public async Task<ActionResult<ProductDto>> GetById(int id)
    //{
    //    var product = await _productService.GetByIdAsync(id);

    //    if (product is null)
    //        return NotFound();

    //    return Ok(product);
    //}

    [HttpGet("details/{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var product = await _productService.GetDetailsAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? type, [FromQuery] string? franchise)
    {
        var results = await _productService.SearchAsync(name, type, franchise);
        return Ok(results);
    }
}
