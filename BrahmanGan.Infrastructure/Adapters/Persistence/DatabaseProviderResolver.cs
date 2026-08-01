using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

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

    /// <summary>
    /// Normaliza una cadena de conexión de PostgreSQL al formato clave-valor de Npgsql.
    /// </summary>
    /// <remarks>
    /// Los hostings gestionados (Render, Heroku, Railway…) publican la cadena como URI
    /// —<c>postgresql://usuario:password@host:puerto/basedatos</c>—, formato que Npgsql no
    /// sabe parsear: falla con «Format of the initialization string does not conform to
    /// specification starting at index 0». Aquí se traduce a
    /// <c>Host=…;Port=…;Database=…;Username=…;Password=…</c> para poder pegar tal cual el
    /// valor que da el proveedor.
    /// <para>
    /// Una cadena que ya venga en formato clave-valor se devuelve intacta, igual que
    /// <c>null</c> o vacío. Los parámetros del query string (<c>?sslmode=require</c>) se
    /// trasladan a sus claves equivalentes; Npgsql valida los nombres y rechaza los que no
    /// reconoce, en vez de descartarlos en silencio.
    /// </para>
    /// </remarks>
    public static string? NormalizarCadenaPostgres(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            return connectionString;

        var cadena = connectionString.Trim();
        if (!cadena.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) &&
            !cadena.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
            return connectionString;

        var uri = new Uri(cadena);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            // Un esquema no estándar deja Uri.Port en -1 cuando no se indica puerto.
            Port = uri.Port > 0 ? uri.Port : 5432
        };

        if (uri.AbsolutePath.Trim('/') is { Length: > 0 } database)
            builder.Database = database;

        // Usuario y contraseña llegan percent-encoded dentro del UserInfo.
        var credenciales = uri.UserInfo.Split(':', 2);
        if (credenciales.Length > 0 && credenciales[0].Length > 0)
            builder.Username = Uri.UnescapeDataString(credenciales[0]);
        if (credenciales.Length > 1 && credenciales[1].Length > 0)
            builder.Password = Uri.UnescapeDataString(credenciales[1]);

        foreach (var parametro in uri.Query.TrimStart('?')
                     .Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var partes = parametro.Split('=', 2);
            if (partes.Length != 2)
                continue;

            builder[Uri.UnescapeDataString(partes[0])] = Uri.UnescapeDataString(partes[1]);
        }

        return builder.ConnectionString;
    }

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
                // Acepta tanto el formato clave-valor como la URI que publican Render y
                // compañía, para todos los DbContext que pasen por aquí.
                options.UseNpgsql(NormalizarCadenaPostgres(connectionString),
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
