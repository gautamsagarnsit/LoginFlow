# LoginFlow Docker Setup Guide

## Prerequisites

- Docker Desktop installed
- Docker Compose installed (included with Docker Desktop)

## Building and Running with Docker Compose

The easiest way to run the application with PostgreSQL is using Docker Compose:

```bash
# Build and start the containers
docker-compose up -d

# View logs
docker-compose logs -f web

# Stop the containers
docker-compose down

# Stop and remove volumes (cleans up database)
docker-compose down -v
```

## Building Docker Image Manually

```bash
# Build the image
docker build -t loginflow:latest .

# Run the container (requires external PostgreSQL)
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Host=your-db-host;Port=5432;Database=loginflow;Username=postgres;Password=postgres;" \
  --name loginflow \
  loginflow:latest
```

## Running Only the Web App

If you have PostgreSQL running elsewhere, you can run just the web container:

```bash
docker run -d \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Host=your-postgres-host;Port=5432;Database=loginflow;Username=postgres;Password=postgres;" \
  loginflow:latest
```

## Accessing the Application

- Application URL: `http://localhost:8080`
- PostgreSQL: `localhost:5432` (when using docker-compose)

## Environment Variables

The application respects the following environment variables:

- `ASPNETCORE_ENVIRONMENT`: Set to `Production`, `Development`, or `Staging`
- `ASPNETCORE_URLS`: Application URL (default: `http://+:8080`)
- `ConnectionStrings__DefaultConnection`: PostgreSQL connection string

## Database Migrations

To run migrations inside the container:

```bash
docker exec loginflow-web dotnet ef database update
```

Or during development, you can run migrations before building:

```bash
dotnet ef database update
```

## Dockerfile Details

The Dockerfile uses a multi-stage build process:

1. **Build Stage**: Uses `dotnet/sdk:10.0` to compile the application
2. **Publish Stage**: Publishes the application in Release configuration
3. **Runtime Stage**: Uses `dotnet/aspnet:10.0` for minimal production image

This approach reduces the final image size by excluding build tools.

## Troubleshooting

### Application won't start
- Check logs: `docker-compose logs web`
- Ensure PostgreSQL is healthy: `docker-compose logs db`
- Verify connection string in docker-compose.yml

### Database connection issues
- Verify the connection string matches your PostgreSQL credentials
- Check that the database container is running and healthy
- Ensure the network is correctly configured

### Port already in use
- Change ports in docker-compose.yml (e.g., `8081:8080` maps host port 8081 to container port 8080)
