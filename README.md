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

From the `PartsFlowERP` directory, start PostgreSQL:

```bash
docker compose up -d postgres
```

Restore and run the backend:

```bash
cd backend/PartsFlow.Api
dotnet restore
dotnet run
```

Run the frontend in a separate terminal:

```bash
cd frontend/partsflow-web
npm install
npm run dev
```

The API health endpoint is available at `http://localhost:5000/api/health` (or the port printed by `dotnet run`). Swagger is available at `/swagger` in the Development environment.
