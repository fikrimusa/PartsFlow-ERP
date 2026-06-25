namespace PartsFlow.Api.Models;

public class Product
{
    public int Id { get; set; }
    public required string SKU { get; set; }
    public required string Name { get; set; }
    public required string Brand { get; set; }
    public required string Category { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public int MinimumStockLevel { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<StockMovement> StockMovements { get; set; } = [];
    public ICollection<SalesOrderItem> SalesOrderItems { get; set; } = [];
}
