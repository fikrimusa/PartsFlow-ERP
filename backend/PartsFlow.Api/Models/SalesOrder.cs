namespace PartsFlow.Api.Models;

public class SalesOrder
{
    public int Id { get; set; }
    public required string OrderNumber { get; set; }
    public required string CustomerName { get; set; }
    public required string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }

    public User CreatedByUser { get; set; } = null!;
    public ICollection<SalesOrderItem> Items { get; set; } = [];
}
