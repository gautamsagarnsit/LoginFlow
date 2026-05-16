# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["LoginFlow/LoginFlow.csproj", "LoginFlow/"]
RUN dotnet restore "LoginFlow/LoginFlow.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
WORKDIR "/src/LoginFlow"
RUN dotnet build "LoginFlow.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "LoginFlow.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install curl for health checks (optional but recommended)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files from publish stage
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080
EXPOSE 8443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "LoginFlow.dll"]
