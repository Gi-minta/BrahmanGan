using System.Net;
using Xunit;

namespace BrahmanGan.IntegrationTests.Endpoints;

public class HealthEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public HealthEndpointsTests(ApiFactory factory) => _factory = factory;

    [SkippableFact]
    public async Task Health_live_devuelve_200()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/health/live");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [SkippableFact]
    public async Task Health_devuelve_200_healthy()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        Assert.Contains("Healthy", await resp.Content.ReadAsStringAsync());
    }
}
