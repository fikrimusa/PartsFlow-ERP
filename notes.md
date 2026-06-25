# PartsFlow ERP Notes

PartsFlow ERP is a simple full-stack portfolio project for auto parts inventory management.

Current scope:

- Product CRUD
- Low-stock status
- Simple dashboard
- Swagger API testing
- PostgreSQL database through EF Core

Not implemented yet:

- Authentication
- Stock movement
- Sales orders
- Purchase orders
- Supplier management
- Deployment

## Backend overview

Backend folder:

```text
backend/PartsFlow.Api/
├── Controllers/
├── Data/
├── DTOs/
├── Migrations/
├── Models/
├── Services/
├── Program.cs
├── PartsFlow.Api.csproj
└── appsettings.json
```

## Main backend files

### `Program.cs`

Starts the ASP.NET Core API.

It configures:

- controllers
- Swagger
- CORS for the Next.js frontend
- PostgreSQL through EF Core
- `ProductService`

### `Data/AppDbContext.cs`

This is the EF Core database context.

It defines:

```csharp
public DbSet<Product> Products => Set<Product>();
```

It also configures:

- unique SKU index
- decimal precision for prices
- seed product data

### `Models/Product.cs`

Represents the `Products` database table.

Important fields:

- `SKU`
- `Name`
- `Brand`
- `Category`
- `Quantity`
- `MinimumStockLevel`
- `CostPrice`
- `SellingPrice`

### `DTOs/`

DTOs define what the API accepts and returns.

- `CreateProductRequest`
- `UpdateProductRequest`
- `ProductResponse`

### `Services/ProductService.cs`

Contains product business logic:

- get all products
- get product by ID
- create product
- update product
- delete product
- get low-stock products

Keeping this logic in a service keeps the controller simple.

### `Controllers/ProductsController.cs`

Defines the Product API routes:

```text
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
GET    /api/products/low-stock
```

## Frontend overview

Frontend folder:

```text
frontend/partsflow-web/
├── src/lib/api.ts
├── src/pages/index.tsx
├── src/pages/dashboard.tsx
└── src/pages/products.tsx
```

### `src/lib/api.ts`

Central API client.

Uses:

```text
NEXT_PUBLIC_API_BASE_URL
```

Default:

```text
http://localhost:5000
```

### `src/pages/dashboard.tsx`

Shows:

- Total Products
- Low Stock Products
- Total Inventory Quantity

### `src/pages/products.tsx`

Shows:

- Product table
- Low Stock / In Stock badge
- Create form
- Edit form
- Delete action

## Low-stock rule

A product is low stock when:

```text
Quantity <= MinimumStockLevel
```

## Interview demo flow

1. Start PostgreSQL.
2. Run backend.
3. Open Swagger and show Product endpoints.
4. Run frontend.
5. Open Dashboard.
6. Open Products page.
7. Create a product.
8. Edit the product quantity.
9. Show low-stock badge changing.
10. Delete the product.

## Run commands

Use three terminals when testing locally:

- Terminal 1: database/backend setup
- Terminal 2: backend server
- Terminal 3: frontend server

### 1. Start PostgreSQL

From project root:

```bash
cd ~/Projects/PartsFlow-ERP
docker compose up -d postgres
```

Check that PostgreSQL is running:

```bash
docker compose ps
```

Expected result:

```text
partsflow-postgres   Up   0.0.0.0:5432->5432/tcp
```

### 2. Apply database migration

From the backend folder:

```bash
cd backend/PartsFlow.Api
dotnet ef database update
```

This creates the `Products` table and inserts demo products.

If your local database is old or broken, reset it:

```bash
cd ~/Projects/PartsFlow-ERP
docker compose down -v
docker compose up -d postgres
cd backend/PartsFlow.Api
dotnet ef database update
```

Only use `docker compose down -v` when you are okay deleting the local database data.

### 3. Run backend

From:

```bash
cd ~/Projects/PartsFlow-ERP/backend/PartsFlow.Api
```

Run:

```bash
dotnet run --urls http://localhost:5000
```

Expected output:

```text
Now listening on: http://localhost:5000
Hosting environment: Development
```

Open Swagger:

```text
http://localhost:5000/swagger
```

Test these endpoints in Swagger:

```text
GET    /api/health
GET    /api/products
GET    /api/products/low-stock
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

### 4. Run frontend

Open a new terminal:

```bash
cd ~/Projects/PartsFlow-ERP
cd frontend/partsflow-web
npm run dev
```

If dependencies are missing:

```bash
npm install
npm run dev
```

Open:

```text
http://localhost:3000
```

## Full backend-to-frontend test

### Test Dashboard

1. Start PostgreSQL.
2. Apply migrations.
3. Run backend on `http://localhost:5000`.
4. Run frontend on `http://localhost:3000`.
5. Open:

```text
http://localhost:3000/dashboard
```

You should see:

- Total Products
- Low Stock Products
- Total Inventory Quantity

These values come from:

```text
GET http://localhost:5000/api/products
```

### Test Products page

Open:

```text
http://localhost:3000/products
```

You should see a table with seeded products:

- RCB Brake Caliper
- UMA Racing Camshaft
- KYT Helmet Visor
- Motorcycle Chain 428H
- Engine Oil 10W-40
- Rear Sprocket 36T

### Test create product

In the product form, enter:

```text
SKU: TEST-001
Name: Test Brake Pad
Brand: Brembo
Category: Brake System
Description: Front brake pad test item
Quantity: 10
Minimum Stock: 5
Cost Price: 20
Selling Price: 35
```

Click:

```text
Create Product
```

Expected result:

- Product appears in the table.
- Status shows `In Stock`.

### Test edit product

Click:

```text
Edit
```

Change:

```text
Quantity: 3
Minimum Stock: 5
```

Click:

```text
Update Product
```

Expected result:

- Quantity becomes `3`.
- Status changes to `Low Stock`.

### Test delete product

Click:

```text
Delete
```

Confirm the browser popup.

Expected result:

- Product is removed from the table.

## Quick curl tests

With the backend running:

```bash
curl http://localhost:5000/api/health
```

```bash
curl http://localhost:5000/api/products
```

```bash
curl http://localhost:5000/api/products/low-stock
```

## Common problems

### `dotnet ef database update` hangs

Stop it with:

```text
Ctrl+C
```

Do not use `Ctrl+Z`, because that pauses the process instead of stopping it.

Then run:

```bash
dotnet build-server shutdown
dotnet build --no-restore /nr:false /p:UseSharedCompilation=false
dotnet ef database update --no-build
```

### Frontend cannot load products

Check:

1. Backend is running on `http://localhost:5000`.
2. PostgreSQL is running.
3. Migration was applied.
4. Browser console does not show CORS errors.

The backend already allows CORS from:

```text
http://localhost:3000
```

### Port already in use

For backend:

```bash
sudo ss -ltnp '( sport = :5000 )'
```

For frontend:

```bash
sudo ss -ltnp '( sport = :3000 )'
```

For PostgreSQL:

```bash
sudo ss -ltnp '( sport = :5432 )'
```
