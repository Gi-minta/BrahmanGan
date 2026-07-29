# syntax=docker/dockerfile:1

# ── Build ──────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia primero los .csproj para aprovechar la caché de restore.
COPY BrahmanGan.API/BrahmanGan.API.csproj BrahmanGan.API/
COPY BrahmanGan.Application/BrahmanGan.Application.csproj BrahmanGan.Application/
COPY BrahmanGan.Domain/BrahmanGan.Domain.csproj BrahmanGan.Domain/
COPY BrahmanGan.Infrastructure/BrahmanGan.Infrastructure.csproj BrahmanGan.Infrastructure/
COPY BrahmanGan.Infrastructure.Migrations.SqlServer/BrahmanGan.Infrastructure.Migrations.SqlServer.csproj BrahmanGan.Infrastructure.Migrations.SqlServer/
COPY BrahmanGan.Infrastructure.Migrations.Postgres/BrahmanGan.Infrastructure.Migrations.Postgres.csproj BrahmanGan.Infrastructure.Migrations.Postgres/
RUN dotnet restore BrahmanGan.API/BrahmanGan.API.csproj

# Copia el resto del código y publica.
COPY . .
RUN dotnet publish BrahmanGan.API/BrahmanGan.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
# Las imágenes .NET 10 escuchan en 8080 por defecto y corren como usuario no-root.
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BrahmanGan.API.dll"]
