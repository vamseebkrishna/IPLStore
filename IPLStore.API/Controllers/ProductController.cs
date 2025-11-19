using IPLStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/products
    // Product list page
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _db.Products.AsNoTracking().ToListAsync();
        return Ok(products);
    }

    // GET: api/products/5
    // Product details page
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }
    // GET: api/products/search?name=&type=&franchise=
    // Search feature (by name, type, franchise)
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] string? franchise)
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(p => p.Type == type);
        }

        if (!string.IsNullOrWhiteSpace(franchise))
        {
            query = query.Where(p => p.Franchise == franchise);
        }

        var results = await query.AsNoTracking().ToListAsync();
        return Ok(results);
    }
}
