using IPLStore.Application.Interfaces.Repositories;
using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _db;

    public CartRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<CartItem>> GetCartItemsAsync(string userId) =>
        _db.CartItems
           .Where(ci => ci.UserId == userId)
           .Include(ci => ci.Product)
           .ToListAsync();

    public Task<CartItem?> GetCartItemAsync(string userId, int productId) =>
        _db.CartItems
           .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

    public Task AddAsync(CartItem item)
    {
        _db.CartItems.Add(item);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CartItem item)
    {
        _db.CartItems.Update(item);
        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<CartItem> items)
    {
        _db.CartItems.RemoveRange(items);
        return Task.CompletedTask;
    }
}
