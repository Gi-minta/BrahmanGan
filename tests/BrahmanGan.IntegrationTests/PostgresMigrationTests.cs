using BrahmanGan.Infrastructure.Adapters.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BrahmanGan.IntegrationTests;

/// <summary>
/// Tests de integración que aplican las migraciones de PostgreSQL contra una base real.
/// En CI, un servicio de PostgreSQL provee la conexión (TEST_POSTGRES_CONNECTION).
/// Si no hay PostgreSQL disponible, los tests se omiten (SkippableFact) en lugar de fallar.
/// </summary>
public class PostgresMigrationTests
{
    private static string ConnectionString =>
        Environment.GetEnvironmentVariable("TEST_POSTGRES_CONNECTION")
        ?? "Host=localhost;Port=5432;Database=BrahmanGanIntegration;Username=postgres;Password=postgres";

    private static ApplicationDbContext CrearContexto()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        DatabaseProviderResolver.Configure(options, DatabaseProvider.Postgres, ConnectionString);
        return new ApplicationDbContext(options.Options);
    }

    [SkippableFact]
    public async Task Migraciones_Postgres_se_aplican_y_siembran_los_roles()
    {
        await using var ctx = CrearContexto();

        Skip.IfNot(await ctx.Database.CanConnectAsync(),
            "PostgreSQL no disponible; se omite el test de integración.");

        await ctx.Database.MigrateAsync();

        // Debe haberse aplicado al menos la migración inicial.
        var aplicadas = (await ctx.Database.GetAppliedMigrationsAsync()).ToList();
        Assert.NotEmpty(aplicadas);

        // La migración inicial siembra los 5 roles de sistema (HasData en RolConfig).
        var totalRoles = await ctx.Roles.CountAsync();
        Assert.True(totalRoles >= 5, $"Se esperaban >= 5 roles sembrados, hubo {totalRoles}.");
    }

    [SkippableFact]
    public async Task El_proveedor_activo_es_Npgsql()
    {
        await using var ctx = CrearContexto();

        Skip.IfNot(await ctx.Database.CanConnectAsync(),
            "PostgreSQL no disponible; se omite el test de integración.");

        Assert.True(ctx.Database.IsNpgsql());
    }
}
