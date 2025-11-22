using IPLStore.Application.DTOs;
using IPLStore.Application.Interfaces;
using IPLStore.Application.Interfaces.Repositories;
using IPLStore.Core.Entities;

namespace IPLStore.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IUnitOfWork _uow;

    public CartService(ICartRepository cartRepo, IUnitOfWork uow)
    {
        _cartRepo = cartRepo;
        _uow = uow;
    }

    public async Task<CartDto> GetCartAsync(string userId)
    {
        var items = await _cartRepo.GetCartItemsAsync(userId);

        return new CartDto
        {
            UserId = userId,
            Items = items.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product!.Name,
                FranchiseName = ci.Product.Franchise,
                Type = ci.Product.Type,
                UnitPrice = ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList()
        };
    }

    public async Task AddToCartAsync(string userId, int productId, int quantity)
    {
        var existing = await _cartRepo.GetCartItemAsync(userId, productId);

        if (existing == null)
        {
            await _cartRepo.AddAsync(new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity += quantity;
            await _cartRepo.UpdateAsync(existing);
        }

        await _uow.SaveChangesAsync();
    }

    public async Task ClearCartAsync(string userId)
    {
        var items = await _cartRepo.GetCartItemsAsync(userId);
        if (items.Count == 0) return;

        await _cartRepo.RemoveRangeAsync(items);
        await _uow.SaveChangesAsync();
    }
}
