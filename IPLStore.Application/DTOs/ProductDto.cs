namespace IPLStore.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string FranchiseName { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
