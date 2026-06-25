namespace PartsFlow.Api.Models;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public required string Type { get; set; }
    public int Quantity { get; set; }
    public required string Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }

    public Product Product { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
}
