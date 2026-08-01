using BrahmanGan.Infrastructure.Adapters.Persistence;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;

namespace BrahmanGan.UnitTests;

public class DatabaseProviderResolverTests
{
    private static IConfiguration Config(params (string Key, string? Value)[] values) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(values.Select(v =>
                new KeyValuePair<string, string?>(v.Key, v.Value)))
            .Build();

    [Theory]
    [InlineData("Postgres", DatabaseProvider.Postgres)]
    [InlineData("postgresql", DatabaseProvider.Postgres)]
    [InlineData("npgsql", DatabaseProvider.Postgres)]
    [InlineData("pgsql", DatabaseProvider.Postgres)]
    [InlineData("SqlServer", DatabaseProvider.SqlServer)]
    [InlineData("cualquier-otra-cosa", DatabaseProvider.SqlServer)]
    [InlineData("", DatabaseProvider.SqlServer)]
    public void Resolve_mapea_el_proveedor(string provider, DatabaseProvider esperado)
    {
        var config = Config(("Database:Provider", provider));

        Assert.Equal(esperado, DatabaseProviderResolver.Resolve(config));
    }

    [Fact]
    public void Resolve_por_defecto_es_SqlServer_cuando_no_hay_clave()
    {
        Assert.Equal(DatabaseProvider.SqlServer, DatabaseProviderResolver.Resolve(Config()));
    }

    [Theory]
    [InlineData(DatabaseProvider.Postgres, "DefaultConnection_Postgres")]
    [InlineData(DatabaseProvider.SqlServer, "DefaultConnection")]
    public void ConnectionStringName_ajusta_el_nombre_segun_proveedor(
        DatabaseProvider provider, string esperado)
    {
        Assert.Equal(esperado, DatabaseProviderResolver.ConnectionStringName("DefaultConnection", provider));
    }

    // ── NormalizarCadenaPostgres ──────────────────────────────────────────

    [Fact]
    public void NormalizarCadenaPostgres_traduce_la_uri_a_formato_clave_valor()
    {
        // Host bajo example.com (RFC 2606) y credenciales evidentemente ficticias: un
        // hostname con forma real de proveedor dispara los escáneres de secretos.
        var uri = "postgresql://USUARIO_DE_PRUEBA:CONTRASENA_DE_PRUEBA@db.example.com:5432/brahman_db";

        var builder = new NpgsqlConnectionStringBuilder(
            DatabaseProviderResolver.NormalizarCadenaPostgres(uri));

        Assert.Equal("db.example.com", builder.Host);
        Assert.Equal(5432, builder.Port);
        Assert.Equal("brahman_db", builder.Database);
        Assert.Equal("USUARIO_DE_PRUEBA", builder.Username);
        Assert.Equal("CONTRASENA_DE_PRUEBA", builder.Password);
    }

    [Theory]
    [InlineData("postgresql://u:p@host/db")]   // esquema largo
    [InlineData("postgres://u:p@host/db")]     // esquema corto
    [InlineData("POSTGRESQL://u:p@host/db")]   // insensible a mayúsculas
    [InlineData("  postgresql://u:p@host/db")] // con espacios por delante
    public void NormalizarCadenaPostgres_reconoce_las_variantes_de_uri(string uri)
    {
        var builder = new NpgsqlConnectionStringBuilder(
            DatabaseProviderResolver.NormalizarCadenaPostgres(uri));

        Assert.Equal("host", builder.Host);
        Assert.Equal("db", builder.Database);
    }

    [Fact]
    public void NormalizarCadenaPostgres_usa_5432_cuando_la_uri_no_trae_puerto()
    {
        var builder = new NpgsqlConnectionStringBuilder(
            DatabaseProviderResolver.NormalizarCadenaPostgres("postgresql://u:p@host/db"));

        Assert.Equal(5432, builder.Port);
    }

    [Fact]
    public void NormalizarCadenaPostgres_decodifica_credenciales_percent_encoded()
    {
        // Una contraseña con @ : / y # debe viajar escapada dentro de la URI.
        var uri = "postgresql://mi%40usuario:p%3Ass%2Fw%23rd@host/db";

        var builder = new NpgsqlConnectionStringBuilder(
            DatabaseProviderResolver.NormalizarCadenaPostgres(uri));

        Assert.Equal("mi@usuario", builder.Username);
        Assert.Equal("p:ss/w#rd", builder.Password);
    }

    [Fact]
    public void NormalizarCadenaPostgres_traslada_los_parametros_del_query()
    {
        var builder = new NpgsqlConnectionStringBuilder(
            DatabaseProviderResolver.NormalizarCadenaPostgres(
                "postgresql://u:p@host/db?sslmode=require"));

        Assert.Equal(SslMode.Require, builder.SslMode);
    }

    [Fact]
    public void NormalizarCadenaPostgres_rechaza_parametros_desconocidos()
    {
        // Mejor fallar con un mensaje que nombre la clave que descartarla en silencio.
        Assert.Throws<ArgumentException>(() =>
            DatabaseProviderResolver.NormalizarCadenaPostgres(
                "postgresql://u:p@host/db?parametro_inventado=1"));
    }

    [Fact]
    public void NormalizarCadenaPostgres_deja_intacta_una_cadena_clave_valor()
    {
        const string cadena = "Host=localhost;Port=5432;Database=BrahmanGanDb;Username=postgres;Password=postgres";

        Assert.Same(cadena, DatabaseProviderResolver.NormalizarCadenaPostgres(cadena));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NormalizarCadenaPostgres_deja_intacto_lo_vacio(string? cadena)
    {
        Assert.Equal(cadena, DatabaseProviderResolver.NormalizarCadenaPostgres(cadena));
    }
}
