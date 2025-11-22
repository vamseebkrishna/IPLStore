using IPLStore.Application.DTOs;
using IPLStore.Application.Interfaces;
using IPLStore.Application.Interfaces.Repositories;
using IPLStore.Core.Entities;

namespace IPLStore.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICartRepository _cartRepo;
    private readonly IUnitOfWork _uow;

    public OrderService(
        IOrderRepository orderRepo,
        ICartRepository cartRepo,
        IUnitOfWork uow)
    {
        _orderRepo = orderRepo;
        _cartRepo = cartRepo;
        _uow = uow;
    }

    public async Task<int> CreateOrderAsync(string userId)
    {
        var cartItems = await _cartRepo.GetCartItemsAsync(userId);
        if (!cartItems.Any())
            return -1;

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Items = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product!.Price
            }).ToList(),
            TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product!.Price)
        };

        await _orderRepo.AddAsync(order);
        await _cartRepo.RemoveRangeAsync(cartItems);

        await _uow.SaveChangesAsync();   // one transaction

        return order.Id;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId)
    {
        var orders = await _orderRepo.GetOrdersForUserAsync(userId);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product!.Name,
                FranchiseName = i.Product.Franchise,
                Type = i.Product.Type,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList()
        });
    }
}
