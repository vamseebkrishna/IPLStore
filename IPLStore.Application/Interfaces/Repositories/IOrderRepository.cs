using IPLStore.Core.Entities;

namespace IPLStore.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersForUserAsync(string userId);
    Task AddAsync(Order order);
}
