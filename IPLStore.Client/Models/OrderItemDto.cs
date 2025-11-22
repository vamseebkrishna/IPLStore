namespace IPLStore.Client.Models.Dto;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string FranchiseName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public decimal LineTotal => Price * Quantity;
}
