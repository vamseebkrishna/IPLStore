using IPLStore.Application.DTOs;

namespace IPLStore.Application.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(string userId);
    Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId);
}
