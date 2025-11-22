namespace IPLStore.Application.DTOs;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; } = "";
    public string FranchiseName { get; set; } = "";
    public decimal LineTotal => Price * Quantity;
}
