using IPLStore.Core.Entities;

namespace IPLStore.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<List<CartItem>> GetCartItemsAsync(string userId);
    Task<CartItem?> GetCartItemAsync(string userId, int productId);
    Task AddAsync(CartItem item);
    Task UpdateAsync(CartItem item);
    Task RemoveRangeAsync(IEnumerable<CartItem> items);
}
