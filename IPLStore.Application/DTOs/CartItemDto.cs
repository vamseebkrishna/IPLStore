namespace IPLStore.Application.DTOs;

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string FranchiseName { get; set; } = "";
    public string Type { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
