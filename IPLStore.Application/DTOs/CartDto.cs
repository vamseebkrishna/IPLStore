namespace IPLStore.Application.DTOs;

public class CartDto
{
    public string UserId { get; set; } = "";
    public List<CartItemDto> Items { get; set; } = new();

    public decimal TotalAmount => Items.Sum(i => i.UnitPrice * i.Quantity);
}
