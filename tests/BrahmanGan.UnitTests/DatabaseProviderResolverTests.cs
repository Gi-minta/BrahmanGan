using BrahmanGan.Infrastructure.Adapters.Persistence;
using Microsoft.Extensions.Configuration;
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
}
