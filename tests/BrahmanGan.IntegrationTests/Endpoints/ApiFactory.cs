using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;

namespace BrahmanGan.IntegrationTests.Endpoints;

/// <summary>
/// Arranca la API en memoria (TestServer) apuntando a la base de PostgreSQL de pruebas,
/// con una clave JWT fija y una contraseña de admin conocida para poder autenticarse.
/// </summary>
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    public static string ConnectionString =>
        Environment.GetEnvironmentVariable("TEST_POSTGRES_CONNECTION")
        ?? "Host=localhost;Port=5432;Database=BrahmanGanIntegration;Username=postgres;Password=postgres";

    public const string AdminEmail = "admin@brahmangan.com";
    public const string AdminPassword = "Test@Admin123!";

    // Se fija la configuración por variables de entorno: el ConfigurationManager de la API
    // las lee con prioridad sobre appsettings.json (que trae SqlServer por defecto). Con el
    // modelo de hosting mínimo es la vía más fiable para sobrescribir la config.
    static ApiFactory()
    {
        Environment.SetEnvironmentVariable("Database__Provider", "Postgres");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection_Postgres", ConnectionString);
        Environment.SetEnvironmentVariable("ConnectionStrings__EventStoreConnection_Postgres", ConnectionString);
        // Clave fija: firma y validación de JWT estables entre peticiones.
        Environment.SetEnvironmentVariable("Jwt__SecretKey", "BrahmanGan-Test-Signing-Key-0123456789-abcdef");
        // Contraseña admin conocida (el seed la usa si el admin aún no existe).
        Environment.SetEnvironmentVariable("Seed__Admin__Password", AdminPassword);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }

    /// <summary>True si hay un PostgreSQL alcanzable; permite omitir los tests si no lo hay.</summary>
    public static bool PostgresDisponible()
    {
        try
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
