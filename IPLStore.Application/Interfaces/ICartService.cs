using IPLStore.Application.DTOs;

namespace IPLStore.Application.Interfaces;

public interface ICartService
{
    Task<CartDto> GetCartAsync(string userId);
    Task AddToCartAsync(string userId, int productId, int quantity);
    Task ClearCartAsync(string userId);
}
