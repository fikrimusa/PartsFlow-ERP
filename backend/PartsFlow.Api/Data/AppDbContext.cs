using Microsoft.EntityFrameworkCore;
using PartsFlow.Api.Models;

namespace PartsFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.FullName).HasMaxLength(150).IsRequired();
            entity.Property(user => user.Email).HasMaxLength(150).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
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
