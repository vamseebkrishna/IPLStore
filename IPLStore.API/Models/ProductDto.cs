namespace IPLStore.API.Models;

public class ProductListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FranchiseName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Jersey, Cap, Flag, etc.
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class ProductDetailsDto : ProductListDto
{
    public string Description { get; set; } = string.Empty;
}
