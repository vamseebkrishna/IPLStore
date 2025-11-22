namespace IPLStore.API.Models;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string FranchiseName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public decimal Price { get; set; }  // Price per unit at time of order

    public decimal LineTotal => Price * Quantity;
}

public class OrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}
