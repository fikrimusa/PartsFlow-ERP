# PartsFlow ERP Backend Notes

These notes explain the backend in simple presentation language.

The backend is the server-side part of PartsFlow ERP. It receives HTTP requests, talks to PostgreSQL through Entity Framework Core, applies business logic, and returns JSON responses to the frontend.

Example:

```text
GET /api/products
```

The backend receives this request, reads products from PostgreSQL, and returns a JSON list of products.

## Backend folder

```text
backend/PartsFlow.Api/
├── Controllers/
├── Data/
├── DTOs/
├── Migrations/
├── Models/
├── Services/
├── Properties/
├── Program.cs
├── PartsFlow.Api.csproj
├── appsettings.json
└── appsettings.Development.json
```

## `Program.cs` — application startup

`Program.cs` is the starting point of the API.

When we run:

```bash
dotnet run --urls http://localhost:5000
```

ASP.NET Core starts from `Program.cs`.

Important responsibilities:

```csharp
builder.Services.AddControllers();
```

This enables controller-based API endpoints.

```csharp
builder.Services.AddSwaggerGen();
```

This enables Swagger, which gives us browser-based API documentation at:

```text
http://localhost:5000/swagger
```

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PartsFlowDatabase")));
```

This connects Entity Framework Core to PostgreSQL.

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

This registers `ProductService` so controllers can use it.

```csharp
app.MapControllers();
```

This activates controller routes such as:

```text
/api/health
/api/products
```

## `Controllers/` — API endpoints

Controllers receive HTTP requests and return HTTP responses.

Current controllers:

```text
Controllers/
├── HealthController.cs
└── ProductsController.cs
```

### `HealthController`

This is a simple test endpoint.

```text
GET /api/health
```

Response:

```json
{
  "status": "ok",
  "app": "PartsFlow.Api"
}
```

Purpose: confirm that the API is running.

### `ProductsController`

This exposes the Product CRUD API.

CRUD means:

- Create
- Read
- Update
- Delete

Current product endpoints:

```text
GET    /api/products              Get all products
GET    /api/products/{id}         Get one product by ID
POST   /api/products              Create a product
PUT    /api/products/{id}         Update a product
DELETE /api/products/{id}         Delete a product
GET    /api/products/low-stock    Get products with low stock
```

The controller does not contain the main business logic. It calls `ProductService`.

Simple flow:

```text
HTTP request
    ↓
ProductsController
    ↓
ProductService
    ↓
AppDbContext
    ↓
PostgreSQL
```

## `Data/` — database access

Current file:

```text
Data/AppDbContext.cs
```

`AppDbContext` is the bridge between C# and PostgreSQL.

It tells Entity Framework Core:

- which database tables exist
- which C# model belongs to each table
- how tables are related
- which fields need unique indexes
- which decimal fields need precision
- what seed data should be inserted

Current DbSets:

```csharp
public DbSet<User> Users => Set<User>();
public DbSet<Product> Products => Set<Product>();
public DbSet<StockMovement> StockMovements => Set<StockMovement>();
public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();
```

Each `DbSet` represents a database table.

For example:

```csharp
public DbSet<Product> Products => Set<Product>();
```

means EF Core can query and save records in the `Products` table.

## `Models/` — database entities

Models represent database tables.

Current models:

```text
Models/
├── User.cs
├── Product.cs
├── StockMovement.cs
├── SalesOrder.cs
└── SalesOrderItem.cs
```

### `Product`

Represents an auto part in inventory.

Important fields:

- `SKU`: unique product code
- `Name`: product name
- `Brand`: product brand
- `Category`: product category
- `Quantity`: current stock quantity
- `MinimumStockLevel`: warning level for low stock
- `CostPrice`: how much the company buys it for
- `SellingPrice`: how much the company sells it for
- `CreatedAt`: when it was created
- `UpdatedAt`: when it was last updated

### `User`

Represents a system user.

Auth is not implemented yet, but the model is prepared for future JWT authentication.

Important fields:

- `FullName`
- `Email`
- `PasswordHash`
- `Role`
- `CreatedAt`

### `StockMovement`

Represents stock coming in or going out.

Example:

```text
Product: Engine Oil 10W-40
Type: In
Quantity: 20
Reason: Supplier delivery
```

This is only modeled today. The stock movement API is not implemented yet.

### `SalesOrder`

Represents a customer order.

Example:

```text
OrderNumber: SO-0001
CustomerName: Ahmad Workshop
Status: Draft
TotalAmount: 250.00
```

This is only modeled today. Sales order API logic is not implemented yet.

### `SalesOrderItem`

Represents one product line inside a sales order.

Example:

```text
Sales Order: SO-0001
Product: RCB Brake Caliper
Quantity: 1
UnitPrice: 250.00
LineTotal: 250.00
```

## `DTOs/` — API request and response shapes

DTO means Data Transfer Object.

DTOs define what data the API accepts and returns.

Current product DTOs:

```text
DTOs/
├── CreateProductRequest.cs
├── UpdateProductRequest.cs
└── ProductResponse.cs
```

### `CreateProductRequest`

Used when creating a product with:

```text
POST /api/products
```

It includes validation rules such as:

- SKU is required
- Name is required
- Quantity cannot be negative
- Minimum stock level cannot be negative
- Cost price cannot be negative
- Selling price cannot be negative

### `UpdateProductRequest`

Used when updating a product with:

```text
PUT /api/products/{id}
```

It uses the same validation rules as product creation.

### `ProductResponse`

Used when returning product data to the frontend.

This keeps API responses clean and consistent.

## `Services/` — business logic

Services contain application logic.

Current service files:

```text
Services/
├── IProductService.cs
└── ProductService.cs
```

### `IProductService`

This is an interface. It describes what the product service can do.

Example methods:

```csharp
Task<List<ProductResponse>> GetAllAsync();
Task<ProductResponse?> GetByIdAsync(int id);
Task<ProductResponse> CreateAsync(CreateProductRequest request);
Task<bool> UpdateAsync(int id, UpdateProductRequest request);
Task<bool> DeleteAsync(int id);
```

### `ProductService`

This contains the actual product logic.

Responsibilities:

- get all products
- get one product by ID
- get low-stock products
- create a product
- update a product
- delete a product
- prevent duplicate SKU values
- convert `Product` models into `ProductResponse` DTOs

Why use a service?

Because it keeps controllers clean.

Bad structure:

```text
Controller does everything
```

Better structure:

```text
Controller handles HTTP
Service handles business logic
DbContext handles database access
```

## `Migrations/` — database version history

Migrations are Entity Framework Core files that describe database structure changes.

Current migration:

```text
Migrations/20260625093309_InitialCreate.cs
```

This migration creates:

- `Users`
- `Products`
- `StockMovements`
- `SalesOrders`
- `SalesOrderItems`
- indexes
- relationships
- seed products

Migration command:

```bash
cd backend/PartsFlow.Api
dotnet ef migrations add InitialCreate
```

Apply migration to PostgreSQL:

```bash
dotnet ef database update
```

Think of migrations as Git commits for the database structure.

## Seed data

The initial migration inserts sample products:

- RCB Brake Caliper
- UMA Racing Camshaft
- KYT Helmet Visor
- Motorcycle Chain 428H
- Engine Oil 10W-40
- Rear Sprocket 36T

This makes the API useful immediately after migration.

After running:

```bash
dotnet ef database update
```

we can test:

```bash
curl http://localhost:5000/api/products
```

and see product data without manually inserting records.

## `appsettings.json` — configuration

This file contains the PostgreSQL connection string:

```text
Host=localhost;
Port=5432;
Database=partsflowdb;
Username=partsflow_user;
Password=partsflow_password
```

The backend uses this to connect to the Docker PostgreSQL database.

For portfolio local development, this is acceptable. In production, passwords should be moved to environment variables or a secret manager.

## `PartsFlow.Api.csproj` — packages and project settings

This file defines the .NET project and installed NuGet packages.

Important packages:

| Package | Purpose |
|---|---|
| `Npgsql.EntityFrameworkCore.PostgreSQL` | Allows EF Core to use PostgreSQL |
| `Microsoft.EntityFrameworkCore.Design` | Enables migration commands |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | Prepared for JWT auth later |
| `Swashbuckle.AspNetCore` | Provides Swagger UI |
| `Microsoft.AspNetCore.OpenApi` | Provides OpenAPI support |

## Request flow examples

### Health check flow

```text
Browser / curl
    ↓ GET /api/health
HealthController
    ↓
JSON response
```

Response:

```json
{
  "status": "ok",
  "app": "PartsFlow.Api"
}
```

### Get products flow

```text
Browser / Frontend / Swagger
    ↓ GET /api/products
ProductsController
    ↓ calls
ProductService
    ↓ queries
AppDbContext
    ↓ reads from
PostgreSQL Products table
    ↓ returns
JSON product list
```

### Create product flow

```text
Swagger / Frontend sends POST /api/products
    ↓
ProductsController receives CreateProductRequest
    ↓
ASP.NET validates required fields and negative numbers
    ↓
ProductService checks duplicate SKU
    ↓
ProductService creates Product model
    ↓
AppDbContext saves it to PostgreSQL
    ↓
API returns 201 Created with ProductResponse
```

## Testing commands

Start PostgreSQL:

```bash
docker compose up -d postgres
```

Apply migration:

```bash
cd backend/PartsFlow.Api
dotnet ef database update
```

Run backend:

```bash
dotnet run --urls http://localhost:5000
```

Open Swagger:

```text
http://localhost:5000/swagger
```

Test products:

```bash
curl http://localhost:5000/api/products
```

Test low stock:

```bash
curl http://localhost:5000/api/products/low-stock
```

Stop PostgreSQL:

```bash
docker compose stop postgres
```

## Presentation summary

For Day 2, the backend now has a real database foundation.

The system has models for users, products, stock movements, and sales orders. Entity Framework Core maps these models to PostgreSQL tables. The Product feature is the first complete CRUD feature, using DTOs for clean API contracts and a service layer to keep business logic out of the controller.

Authentication, frontend product pages, stock movement logic, and sales order logic are intentionally not implemented yet.
