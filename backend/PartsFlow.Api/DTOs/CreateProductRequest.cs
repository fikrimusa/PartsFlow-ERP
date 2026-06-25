using System.ComponentModel.DataAnnotations;

namespace PartsFlow.Api.DTOs;

public class CreateProductRequest
{
    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@".*\S.*", ErrorMessage = "SKU is required.")]
    public string SKU { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Minimum stock level cannot be negative.")]
    public int MinimumStockLevel { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Cost price cannot be negative.")]
    public decimal CostPrice { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Selling price cannot be negative.")]
    public decimal SellingPrice { get; set; }
}
