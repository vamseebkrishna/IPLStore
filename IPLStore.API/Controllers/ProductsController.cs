using IPLStore.API.Models;
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductListDto>>> GetAll()
    {
        var products = await _db.Products
            .AsNoTracking()
            .ToListAsync();

        var result = products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            FranchiseName = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        }).ToList();

        return Ok(result);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailsDto>> GetById(int id)
    {
        var p = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (p == null)
            return NotFound();

        return Ok(new ProductDetailsDto
        {
            Id = p.Id,
            Name = p.Name,
            FranchiseName = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Description = p.Description
        });
    }

    // GET: api/products/search
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductListDto>>> Search(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] string? franchise)
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        if (!string.IsNullOrWhiteSpace(franchise))
            query = query.Where(p => p.Franchise == franchise);

        var products = await query.AsNoTracking().ToListAsync();

        var result = products.Select(p => new ProductListDto
        {
            Id = p.Id,
            Name = p.Name,
            FranchiseName = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        }).ToList();

        return Ok(result);
    }
}
