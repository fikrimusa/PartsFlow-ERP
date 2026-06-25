using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartsFlow.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    MinimumStockLevel = table.Column<int>(type: "integer", nullable: false),
                    CostPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SellingPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CostPrice", "CreatedAt", "Description", "MinimumStockLevel", "Name", "Quantity", "SKU", "SellingPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "RCB", "Brake System", 180.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Performance brake caliper for motorcycles.", 5, "RCB Brake Caliper", 12, "RCB-BR-CAL-001", 250.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "UMA Racing", "Engine", 220.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Racing camshaft for performance engine builds.", 3, "UMA Racing Camshaft", 8, "UMA-CAM-001", 320.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "KYT", "Accessories", 35.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Replacement helmet visor.", 10, "KYT Helmet Visor", 25, "KYT-VISOR-001", 59.90m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Generic", "Drivetrain", 45.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Heavy-duty 428H motorcycle chain.", 6, "Motorcycle Chain 428H", 18, "CHAIN-428H-001", 75.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Motul", "Lubricants", 18.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Semi-synthetic motorcycle engine oil.", 15, "Engine Oil 10W-40", 40, "OIL-10W40-001", 32.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, "SSS", "Drivetrain", 38.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "36-tooth rear sprocket.", 5, "Rear Sprocket 36T", 4, "SPROCKET-36T-001", 65.00m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
