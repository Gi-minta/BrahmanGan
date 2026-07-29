# Migraciones PostgreSQL

Contiene el set de migraciones EF Core para **PostgreSQL** (contexto `ApplicationDbContext`).
La migración inicial (`*_Inicial`) ya está generada y crea el esquema completo con tipos
nativos de Postgres (`text`, `timestamp without time zone`, `numeric`,
`Npgsql:ValueGenerationStrategy`).

Al arrancar la API con `Database:Provider=Postgres`, `DbInitializer` aplica automáticamente
estas migraciones (`Database.MigrateAsync`) y siembra el usuario administrador.

## Regenerar / añadir migraciones

Requiere el SDK de .NET 10 y `dotnet-ef`, con `Database:Provider=Postgres` en la configuración
activa (o `Database__Provider=Postgres` como variable de entorno):

```bash
dotnet ef migrations add <Nombre> \
  --project BrahmanGan.Infrastructure.Migrations.Postgres \
  --startup-project BrahmanGan.API \
  --context ApplicationDbContext
```

Verifica que el SQL use tipos de PostgreSQL y no de SQL Server (`nvarchar`, `datetime2`,
`SqlServer:Identity`, `GETUTCDATE`/`GETDATE`).
