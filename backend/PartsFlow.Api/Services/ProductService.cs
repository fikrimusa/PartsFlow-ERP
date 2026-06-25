using Microsoft.EntityFrameworkCore;
using PartsFlow.Api.Data;
using PartsFlow.Api.DTOs;
using PartsFlow.Api.Models;

namespace PartsFlow.Api.Services;

public class ProductService(AppDbContext dbContext) : IProductService
{
    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await dbContext.Products
            .OrderBy(product => product.Name)
            .ToListAsync();

        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = await dbContext.Products.FindAsync(id);

        return product is null ? null : ToResponse(product);
    }

    public async Task<List<ProductResponse>> GetLowStockAsync()
    {
        var products = await dbContext.Products
            .Where(product => product.Quantity <= product.MinimumStockLevel)
            .OrderBy(product => product.Quantity)
            .ThenBy(product => product.Name)
            .ToListAsync();

        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var sku = request.SKU.Trim();

        if (await dbContext.Products.AnyAsync(product => product.SKU == sku))
        {
            throw new InvalidOperationException("A product with this SKU already exists.");
        }

        var now = DateTime.UtcNow;

        var product = new Product
        {
            SKU = sku,
            Name = request.Name.Trim(),
            Brand = request.Brand.Trim(),
            Category = request.Category.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Quantity = request.Quantity,
            MinimumStockLevel = request.MinimumStockLevel,
            CostPrice = request.CostPrice,
            SellingPrice = request.SellingPrice,
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return ToResponse(product);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }

        var sku = request.SKU.Trim();

        var skuBelongsToAnotherProduct = await dbContext.Products
            .AnyAsync(existingProduct => existingProduct.Id != id && existingProduct.SKU == sku);

        if (skuBelongsToAnotherProduct)
        {
            throw new InvalidOperationException("A product with this SKU already exists.");
        }

        product.SKU = sku;
        product.Name = request.Name.Trim();
        product.Brand = request.Brand.Trim();
        product.Category = request.Category.Trim();
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        product.Quantity = request.Quantity;
        product.MinimumStockLevel = request.MinimumStockLevel;
        product.CostPrice = request.CostPrice;
        product.SellingPrice = request.SellingPrice;
        product.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return true;
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            SKU = product.SKU,
            Name = product.Name,
            Brand = product.Brand,
            Category = product.Category,
            Description = product.Description,
            Quantity = product.Quantity,
            MinimumStockLevel = product.MinimumStockLevel,
            CostPrice = product.CostPrice,
            SellingPrice = product.SellingPrice,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}
