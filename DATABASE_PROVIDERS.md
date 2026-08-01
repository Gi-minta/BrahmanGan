# Proveedores de base de datos (SQL Server / PostgreSQL)

BrahmanGan soporta **dos proveedores de base de datos** de forma intercambiable mediante
configuración: **SQL Server** (por defecto) y **PostgreSQL**. El modelo de datos es el mismo;
solo cambian el proveedor EF Core, la cadena de conexión y el set de migraciones.

## Probar con PostgreSQL rápidamente (Docker)

Hay un `docker-compose.yml` en la raíz que levanta PostgreSQL + la API ya configurada:

```bash
docker compose up --build
```

La API queda en http://localhost:8080 (Scalar UI en `/scalar`). Al arrancar aplica las
migraciones de Postgres y siembra el usuario admin por defecto (email
`admin@brahmangan.com`; la contraseña por defecto se define en `DbInitializer` y debe
cambiarse tras el primer inicio de sesión). Prueba el login:

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"Email":"admin@brahmangan.com","Password":"<PASSWORD_ADMIN>"}'
```

Devuelve un JWT (`accessToken`). Nota: al usar la imagen slim de .NET puede aparecer un
warning inofensivo de Npgsql (`libgssapi_krb5.so.2: cannot open shared object file`) al
sondear autenticación Kerberos; la conexión por usuario/contraseña funciona igualmente.

## Cómo elegir el proveedor

En `appsettings.json` (o por variable de entorno / secreto):

```jsonc
{
  "Database": {
    "Provider": "SqlServer"   // "SqlServer" (por defecto) o "Postgres"
  }
}
```

También por variable de entorno: `Database__Provider=Postgres`.

## Cadenas de conexión

Se leen según el proveedor activo:

| Proveedor | Cadena principal | Event Store |
|-----------|------------------|-------------|
| SqlServer | `ConnectionStrings:DefaultConnection` | `ConnectionStrings:EventStoreConnection` |
| Postgres  | `ConnectionStrings:DefaultConnection_Postgres` | `ConnectionStrings:EventStoreConnection_Postgres` |

Ejemplo Postgres:

```jsonc
"ConnectionStrings": {
  "DefaultConnection_Postgres": "Host=localhost;Port=5432;Database=BrahmanGanDb;Username=postgres;Password=****",
  "EventStoreConnection_Postgres": "Host=localhost;Port=5432;Database=BrahmanGanEventStore;Username=postgres;Password=****"
}
```

> No commitees credenciales reales. Los valores versionados usan `CHANGE_ME` como marcador.

## Migraciones (una por proveedor)

Las migraciones EF Core **no son portables** entre proveedores, por eso cada uno tiene su
propio proyecto/assembly y el proveedor activo determina cuál se aplica en runtime:

- `BrahmanGan.Infrastructure.Migrations.SqlServer` — migraciones para SQL Server (ya existentes).
- `BrahmanGan.Infrastructure.Migrations.Postgres` — migraciones para PostgreSQL.

La selección de proveedor, cadena y assembly de migraciones está centralizada en
`BrahmanGan.Infrastructure/Adapters/Persistence/DatabaseProviderResolver.cs`.

Cada proyecto contiene **dos sets de migraciones independientes**, uno por `DbContext`:

| Contexto | Ubicación dentro del proyecto | Tablas |
|----------|-------------------------------|--------|
| `ApplicationDbContext` | `Migrations/` | Esquema ganadero completo |
| `EventStoreDbContext`  | `Migrations/EventStoreDb/` | `DomainEvents` |

### Generar / regenerar migraciones

Requiere el **SDK de .NET 10** y `dotnet-ef`. Ejecuta con el proveedor deseado activo:

```bash
# PostgreSQL (con Database:Provider=Postgres)
dotnet ef migrations add Inicial \
  --project BrahmanGan.Infrastructure.Migrations.Postgres \
  --startup-project BrahmanGan.API

# SQL Server (con Database:Provider=SqlServer)
dotnet ef migrations add <Nombre> \
  --project BrahmanGan.Infrastructure.Migrations.SqlServer \
  --startup-project BrahmanGan.API
```

Para el event store hay que indicar el contexto y su carpeta, ya que el proyecto alberga dos:

```bash
dotnet ef migrations add <Nombre> \
  --project BrahmanGan.Infrastructure.Migrations.<Proveedor> \
  --startup-project BrahmanGan.API \
  --context EventStoreDbContext \
  --output-dir Migrations/EventStoreDb
```

Al arrancar, `DbInitializer.InicializarAsync` aplica automáticamente las migraciones
pendientes (`Database.MigrateAsync`) de **ambos contextos** para el proveedor activo y
siembra el usuario administrador.

## Notas técnicas

- **Defaults de fecha**: las `Configurations` declaran defaults de SQL Server
  (`GETUTCDATE()` / `GETDATE()`). Cuando el proveedor es PostgreSQL se traducen
  automáticamente (`now() at time zone 'utc'` / `LOCALTIMESTAMP`) en un único lugar
  (`ApplicationDbContext.OnModelCreating`), sin tocar las Configurations.
- **Event Store**: `EventData` usa `text` en Postgres y `nvarchar(max)` en SQL Server.
- **Event Store en la misma base de datos**: si `EventStoreConnection*` apunta a la misma BD
  que `DefaultConnection*` (caso de `docker-compose` y de los hosting con una sola BD, como
  Render), ambos contextos comparten la tabla `__EFMigrationsHistory`. Es correcto: los
  `MigrationId` de cada set son distintos y `MigrateAsync` solo aplica los que faltan de su
  propio assembly. Se migran en secuencia, nunca en paralelo.
- **Timestamps de Npgsql**: se habilita `Npgsql.EnableLegacyTimestampBehavior` para mapear
  `DateTime` a `timestamp without time zone` (equivalente al `datetime2` de SQL Server) y
  evitar el modo estricto de Kind de Npgsql 6+.
- Los tipos `decimal(p,s)` y `char(1)` son portables (Npgsql los mapea a `numeric`/`char`).
