using IPLStore.Application.DTOs;
using IPLStore.Application.Interfaces;
using IPLStore.Application.Interfaces.Repositories;

namespace IPLStore.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _products;

    public ProductService(IProductRepository products)
    {
        _products = products;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var list = await _products.GetAllAsync();

        return list.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            FranchiseName = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        });
    }

    //public async Task<ProductDto?> GetByIdAsync(int id)
    //{
    //    var p = await _products.GetByIdAsync(id);
    //    if (p == null) return null;

    //    return new ProductDto
    //    {
    //        Id = p.Id,
    //        Name = p.Name,
    //        FranchiseName = p.Franchise,
    //        Type = p.Type,
    //        Price = p.Price,
    //        ImageUrl = p.ImageUrl
    //    };
    //}

    public async Task<IEnumerable<ProductDto>> SearchAsync(string? name, string? type, string? franchise)
    {
        var list = await _products.SearchAsync(name, type, franchise);

        return list.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            FranchiseName = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        });
    }

    public async Task<ProductDetailsDto?> GetDetailsAsync(int id)
    {
        var p = await _products.GetByIdAsync(id);
        if (p == null)
            return null;

        return new ProductDetailsDto
        {
            Id = p.Id,
            Name = p.Name,
            Franchise = p.Franchise,
            Type = p.Type,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            Description = p.Description ?? ""
            //Year = p.Year
        };
    }




    // GetDetailsAsync stays similar, using _products.GetByIdAsync(...)
}
