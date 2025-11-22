namespace IPLStore.API.Models;

public class CartItemDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string FranchiseName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal LineTotal => UnitPrice * Quantity;
}

public class CartDto
{
    public string UserId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.LineTotal);
}
