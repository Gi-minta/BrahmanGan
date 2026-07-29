# Migraciones PostgreSQL

Esta carpeta contendrá el set de migraciones EF Core para **PostgreSQL**.

Aún está vacía porque las migraciones se generan con el SDK de .NET 10 (no disponible en
el entorno donde se preparó el soporte de doble proveedor). Genera la migración inicial en
una máquina con el SDK y acceso a NuGet:

```bash
# Desde la raíz del repositorio, con Database:Provider=Postgres en la configuración activa
# (appsettings o variable de entorno Database__Provider=Postgres)
dotnet ef migrations add Inicial \
  --project BrahmanGan.Infrastructure.Migrations.Postgres \
  --startup-project BrahmanGan.API
```

Verifica que el SQL generado use tipos de PostgreSQL (`text`, `timestamp`, `numeric`,
`Npgsql:ValueGenerationStrategy`) y no tipos de SQL Server (`nvarchar`, `datetime2`,
`SqlServer:Identity`, `GETUTCDATE`/`GETDATE`).

Al arrancar la API con `Database:Provider=Postgres`, `DbInitializer` aplica automáticamente
estas migraciones (`Database.MigrateAsync`) y siembra el usuario administrador.
