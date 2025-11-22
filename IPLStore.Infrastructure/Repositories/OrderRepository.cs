using IPLStore.Application.Interfaces.Repositories;
using IPLStore.Core.Entities;
using IPLStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IPLStore.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Order>> GetOrdersForUserAsync(string userId) =>
        _db.Orders
           .Where(o => o.UserId == userId)
           .Include(o => o.Items)
           .ThenInclude(i => i.Product)
           .OrderByDescending(o => o.OrderDate)
           .ToListAsync();

    public Task AddAsync(Order order)
    {
        _db.Orders.Add(order);
        return Task.CompletedTask;
    }
}
