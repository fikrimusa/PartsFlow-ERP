# PartsFlow ERP

PartsFlow ERP is a portfolio project for an auto-parts trading and distribution company. It will provide a focused system for managing inventory, stock movements, and sales orders.

## Tech stack

- Backend: C# with ASP.NET Core Web API
- Database: PostgreSQL with Entity Framework Core
- API documentation: Swagger / OpenAPI
- Authentication: JWT (planned)
- Frontend: Next.js, TypeScript, and Tailwind CSS
- Local infrastructure: Docker Compose

## Planned MVP features

- Product catalog management
- Inventory and stock movement tracking
- Sales order management
- Dashboard summaries
- JWT-based user authentication

## Setup

From the `PartsFlow-ERP` directory, start PostgreSQL:

```bash
docker compose up -d postgres
```

Restore and run the backend:

```bash
cd backend/PartsFlow.Api
dotnet restore
dotnet run
```

Apply database migrations:

```bash
cd backend/PartsFlow.Api
dotnet ef database update
```

Run the frontend in a separate terminal:

```bash
cd frontend/partsflow-web
npm install
npm run dev
```

The API health endpoint is available at `http://localhost:5000/api/health` (or the port printed by `dotnet run`). Swagger is available at `/swagger` in the Development environment.

## Database setup

The local PostgreSQL database is defined in `docker-compose.yml`.

- Database: `partsflowdb`
- Username: `partsflow_user`
- Password: `partsflow_password`
- Port: `5432`

Useful database commands:

```bash
# Start PostgreSQL
docker compose up -d postgres

# Stop PostgreSQL without deleting data
docker compose stop postgres

# Apply EF Core migrations
cd backend/PartsFlow.Api
dotnet ef database update

# Create a new migration
dotnet ef migrations add MigrationName
```

## Backend commands

```bash
cd backend/PartsFlow.Api
dotnet build
dotnet run --urls http://localhost:5000
```

## Product API endpoints

- `GET /api/products`
- `GET /api/products/{id}`
- `POST /api/products`
- `PUT /api/products/{id}`
- `DELETE /api/products/{id}`
- `GET /api/products/low-stock`
