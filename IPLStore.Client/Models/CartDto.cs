namespace IPLStore.Client.Models.Dto;

public class CartDto
{
    public string UserId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.LineTotal);
}
