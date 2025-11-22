namespace IPLStore.Application.DTOs;

public class ProductDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Franchise { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = "";

    // UI-friendly aliases
    public string FranchiseName { get; set; }
    public string? Description { get; set; }

}
