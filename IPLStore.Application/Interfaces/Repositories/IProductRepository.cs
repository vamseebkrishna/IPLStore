using IPLStore.Core.Entities;

namespace IPLStore.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> SearchAsync(string? name, string? type, string? franchise);
}
