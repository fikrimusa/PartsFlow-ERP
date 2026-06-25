using Microsoft.EntityFrameworkCore;
using PartsFlow.Api.Models;

namespace PartsFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.FullName).HasMaxLength(150).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(255).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(50).IsRequired();
            entity.Property(user => user.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(product => product.SKU).IsUnique();
            entity.Property(product => product.SKU).HasMaxLength(50).IsRequired();
            entity.Property(product => product.Name).HasMaxLength(150).IsRequired();
            entity.Property(product => product.Brand).HasMaxLength(100).IsRequired();
            entity.Property(product => product.Category).HasMaxLength(100).IsRequired();
            entity.Property(product => product.Description).HasMaxLength(1000);
            entity.Property(product => product.CostPrice).HasPrecision(18, 2);
            entity.Property(product => product.SellingPrice).HasPrecision(18, 2);
            entity.Property(product => product.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(product => product.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(stockMovement => stockMovement.Type).HasMaxLength(50).IsRequired();
            entity.Property(stockMovement => stockMovement.Reason).HasMaxLength(255).IsRequired();
            entity.Property(stockMovement => stockMovement.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity
                .HasOne(stockMovement => stockMovement.Product)
                .WithMany(product => product.StockMovements)
                .HasForeignKey(stockMovement => stockMovement.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(stockMovement => stockMovement.CreatedByUser)
                .WithMany(user => user.StockMovements)
                .HasForeignKey(stockMovement => stockMovement.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SalesOrder>(entity =>
        {
            entity.Property(salesOrder => salesOrder.OrderNumber).HasMaxLength(50).IsRequired();
            entity.Property(salesOrder => salesOrder.CustomerName).HasMaxLength(150).IsRequired();
            entity.Property(salesOrder => salesOrder.Status).HasMaxLength(50).IsRequired();
            entity.Property(salesOrder => salesOrder.TotalAmount).HasPrecision(18, 2);
            entity.Property(salesOrder => salesOrder.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity
                .HasOne(salesOrder => salesOrder.CreatedByUser)
                .WithMany(user => user.SalesOrders)
                .HasForeignKey(salesOrder => salesOrder.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SalesOrderItem>(entity =>
        {
            entity.Property(salesOrderItem => salesOrderItem.UnitPrice).HasPrecision(18, 2);
            entity.Property(salesOrderItem => salesOrderItem.LineTotal).HasPrecision(18, 2);

            entity
                .HasOne(salesOrderItem => salesOrderItem.SalesOrder)
                .WithMany(salesOrder => salesOrder.Items)
                .HasForeignKey(salesOrderItem => salesOrderItem.SalesOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(salesOrderItem => salesOrderItem.Product)
                .WithMany(product => product.SalesOrderItems)
                .HasForeignKey(salesOrderItem => salesOrderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedProducts(modelBuilder);
    }

    private static void SeedProducts(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                SKU = "RCB-BR-CAL-001",
                Name = "RCB Brake Caliper",
                Brand = "RCB",
                Category = "Brake System",
                Description = "Performance brake caliper for motorcycles.",
                Quantity = 12,
                MinimumStockLevel = 5,
                CostPrice = 180.00m,
                SellingPrice = 250.00m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 2,
                SKU = "UMA-CAM-001",
                Name = "UMA Racing Camshaft",
                Brand = "UMA Racing",
                Category = "Engine",
                Description = "Racing camshaft for performance engine builds.",
                Quantity = 8,
                MinimumStockLevel = 3,
                CostPrice = 220.00m,
                SellingPrice = 320.00m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 3,
                SKU = "KYT-VISOR-001",
                Name = "KYT Helmet Visor",
                Brand = "KYT",
                Category = "Accessories",
                Description = "Replacement helmet visor.",
                Quantity = 25,
                MinimumStockLevel = 10,
                CostPrice = 35.00m,
                SellingPrice = 59.90m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 4,
                SKU = "CHAIN-428H-001",
                Name = "Motorcycle Chain 428H",
                Brand = "Generic",
                Category = "Drivetrain",
                Description = "Heavy-duty 428H motorcycle chain.",
                Quantity = 18,
                MinimumStockLevel = 6,
                CostPrice = 45.00m,
                SellingPrice = 75.00m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 5,
                SKU = "OIL-10W40-001",
                Name = "Engine Oil 10W-40",
                Brand = "Motul",
                Category = "Lubricants",
                Description = "Semi-synthetic motorcycle engine oil.",
                Quantity = 40,
                MinimumStockLevel = 15,
                CostPrice = 18.00m,
                SellingPrice = 32.00m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Product
            {
                Id = 6,
                SKU = "SPROCKET-36T-001",
                Name = "Rear Sprocket 36T",
                Brand = "SSS",
                Category = "Drivetrain",
                Description = "36-tooth rear sprocket.",
                Quantity = 4,
                MinimumStockLevel = 5,
                CostPrice = 38.00m,
                SellingPrice = 65.00m,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });
    }
}
