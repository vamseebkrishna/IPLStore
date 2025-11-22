using IPLStore.Application.Interfaces.Repositories;
using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products.FirstOrDefaultAsync(p => p.Id == id)!;

    public Task<List<Product>> GetAllAsync() =>
        _db.Products.AsNoTracking().ToListAsync();

    public async Task<List<Product>> SearchAsync(string? name, string? type, string? franchise)
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type.Contains(type));

        if (!string.IsNullOrWhiteSpace(franchise))
            query = query.Where(p => p.Franchise.Contains(franchise));

        return await query.AsNoTracking().ToListAsync();
    }
}
