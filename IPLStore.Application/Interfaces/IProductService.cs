using IPLStore.Application.DTOs;

namespace IPLStore.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    //Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDetailsDto?> GetDetailsAsync(int id);
    Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? type, string? franchise);

}
