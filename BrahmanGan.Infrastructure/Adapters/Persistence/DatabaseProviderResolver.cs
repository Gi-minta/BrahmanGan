using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BrahmanGan.Infrastructure.Adapters.Persistence;

/// <summary>Proveedores de base de datos soportados por BrahmanGan.</summary>
public enum DatabaseProvider
{
    SqlServer = 0,
    Postgres = 1
}

/// <summary>
/// Nombres de los assemblies que contienen las migraciones EF Core de cada proveedor.
/// Las migraciones no son portables entre proveedores, por lo que cada uno vive en su
/// propio proyecto y el proveedor activo determina cuál se usa en runtime y en diseño.
/// </summary>
public static class MigrationsAssemblies
{
    public const string SqlServer = "BrahmanGan.Infrastructure.Migrations.SqlServer";
    public const string Postgres  = "BrahmanGan.Infrastructure.Migrations.Postgres";
}

/// <summary>
/// Centraliza la selección del proveedor de base de datos (SQL Server o PostgreSQL)
/// a partir de la configuración <c>Database:Provider</c> y aplica el <c>DbContextOptionsBuilder</c>
/// correspondiente. Reutilizado por todos los <c>DbContext</c> registrados.
/// </summary>
public static class DatabaseProviderResolver
{
    /// <summary>Lee <c>Database:Provider</c>. Por defecto <see cref="DatabaseProvider.SqlServer"/>.</summary>
    public static DatabaseProvider Resolve(IConfiguration configuration)
    {
        var raw = configuration["Database:Provider"];
        return raw?.Trim().ToLowerInvariant() switch
        {
            "postgres" or "postgresql" or "npgsql" or "pgsql" => DatabaseProvider.Postgres,
            _ => DatabaseProvider.SqlServer
        };
    }

    /// <summary>
    /// Devuelve el nombre de la cadena de conexión a usar según el proveedor.
    /// Para Postgres se prefiere la variante <c>{baseName}_Postgres</c> (formato Host/Username/Password)
    /// y se cae a <c>{baseName}</c> si no existe.
    /// </summary>
    public static string ConnectionStringName(string baseName, DatabaseProvider provider)
        => provider == DatabaseProvider.Postgres ? $"{baseName}_Postgres" : baseName;

    /// <summary>Aplica el proveedor elegido al <c>DbContextOptionsBuilder</c>.</summary>
    public static DbContextOptionsBuilder Configure(
        DbContextOptionsBuilder options,
        DatabaseProvider provider,
        string? connectionString)
    {
        switch (provider)
        {
            case DatabaseProvider.Postgres:
                // Modo de compatibilidad: mapea DateTime a 'timestamp without time zone'
                // (equivalente al datetime2 de SQL Server) y evita el modo estricto de Kind
                // introducido en Npgsql 6+. Debe establecerse antes de usar el DbContext.
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(MigrationsAssemblies.Postgres)
                          .CommandTimeout(60));
                break;

            case DatabaseProvider.SqlServer:
            default:
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(MigrationsAssemblies.SqlServer)
                          .CommandTimeout(60));
                break;
        }

        return options;
    }
}
