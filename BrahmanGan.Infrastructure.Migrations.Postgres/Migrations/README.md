# Migraciones PostgreSQL

Contiene los sets de migraciones EF Core para **PostgreSQL**, uno por cada `DbContext`:

- **`ApplicationDbContext`** — esta carpeta. La migración inicial (`*_Inicial`) crea el
  esquema ganadero completo con tipos nativos de Postgres (`text`,
  `timestamp without time zone`, `numeric`, `Npgsql:ValueGenerationStrategy`).
- **`EventStoreDbContext`** — subcarpeta `EventStoreDb/`. Crea la tabla `DomainEvents` del
  event store con sus índices.

Al arrancar la API con `Database:Provider=Postgres`, `DbInitializer` aplica automáticamente
las migraciones de **ambos contextos** (`Database.MigrateAsync`) y siembra el usuario
administrador.

## Regenerar / añadir migraciones

Requiere el SDK de .NET 10 y `dotnet-ef`, con `Database:Provider=Postgres` en la configuración
activa (o `Database__Provider=Postgres` como variable de entorno):

```bash
dotnet ef migrations add <Nombre> \
  --project BrahmanGan.Infrastructure.Migrations.Postgres \
  --startup-project BrahmanGan.API \
  --context ApplicationDbContext
```

Para el event store hay que añadir además la carpeta de salida, o EF mezclaría los dos sets:

```bash
dotnet ef migrations add <Nombre> \
  --project BrahmanGan.Infrastructure.Migrations.Postgres \
  --startup-project BrahmanGan.API \
  --context EventStoreDbContext \
  --output-dir Migrations/EventStoreDb
```

Verifica que el SQL use tipos de PostgreSQL y no de SQL Server (`nvarchar`, `datetime2`,
`SqlServer:Identity`, `GETUTCDATE`/`GETDATE`).
