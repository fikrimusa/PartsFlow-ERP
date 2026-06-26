# PartsFlow ERP

PartsFlow ERP is a simple full-stack portfolio project for managing auto parts inventory.

The current MVP focuses on Product CRUD: create, view, update, delete, and identify low-stock products.

## Tech stack

- Backend: ASP.NET Core Web API
- Database: PostgreSQL
- ORM: Entity Framework Core
- API docs: Swagger / OpenAPI
- Frontend: Next.js, TypeScript, Tailwind CSS
- Container: Docker Compose

## MVP features

- Product catalog CRUD
- Low-stock status based on quantity and minimum stock level
- Simple inventory dashboard
- PostgreSQL database with EF Core migration
- Seed demo product data

## Frontend pages

- `/` - Dashboard
- `/products` - Product management

## Product CRUD description

Products represent auto parts in inventory.

Each product includes:

- SKU
- Name
- Brand
- Category
- Description
- Quantity
- Minimum stock level
- Cost price
- Selling price

A product is considered low stock when:

```text
Quantity <= MinimumStockLevel
```

## API endpoints

- `GET /api/health`
- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `GET /api/products/low-stock`

## Demo data

The `AddProduct` migration seeds these products:

- RCB Brake Caliper
- UMA Racing Camshaft
- KYT Helmet Visor
- Motorcycle Chain 428H
- Engine Oil 10W-40
- Rear Sprocket 36T

## Setup commands

From the project root:

```bash
docker compose up -d postgres
```

Apply database migration:

```bash
cd backend/PartsFlow.Api
dotnet ef database update
```

## Backend run command

```bash
cd backend/PartsFlow.Api
dotnet run --urls http://localhost:5000
```

Swagger:

```text
http://localhost:5000/swagger
```

## Frontend run command

In a separate terminal:

```bash
cd frontend/partsflow-web
npm install
npm run dev
```

Frontend:

```text
http://localhost:3000
```

The frontend API base URL defaults to:

```text
http://localhost:5000
```

You can override it with:

```bash
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000 npm run dev
```

## Database migration commands

```bash
cd backend/PartsFlow.Api

# Add a migration
dotnet ef migrations add AddProduct

# Apply migrations
dotnet ef database update
```

If your local database contains old development tables and you want a clean reset:

```bash
docker compose down -v
docker compose up -d postgres
cd backend/PartsFlow.Api
dotnet ef database update
```

## Docker command

```bash
docker compose up -d postgres
```

Stop PostgreSQL:

```bash
docker compose stop postgres
```

## Screenshots

Screenshots can be added here later:

- Dashboard page (`/`)
- Products table
- Create/edit product form
- Swagger Product API

## Future improvements

- JWT authentication
- Stock movement
- Sales orders
- Purchase orders
- Supplier management
- Dashboard charts
- Deployment
