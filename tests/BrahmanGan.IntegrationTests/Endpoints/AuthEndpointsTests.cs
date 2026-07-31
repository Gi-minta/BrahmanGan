using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BrahmanGan.IntegrationTests.Endpoints;

public class AuthEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public AuthEndpointsTests(ApiFactory factory) => _factory = factory;

    [SkippableFact]
    public async Task Login_con_credenciales_validas_devuelve_token()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();

        var resp = await client.PostAsJsonAsync("/api/auth/login",
            new { Email = ApiFactory.AdminEmail, Password = ApiFactory.AdminPassword });

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        var token = doc.RootElement.GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [SkippableFact]
    public async Task Login_con_password_incorrecta_no_devuelve_200()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();

        var resp = await client.PostAsJsonAsync("/api/auth/login",
            new { Email = ApiFactory.AdminEmail, Password = "claveIncorrecta" });

        Assert.False(resp.IsSuccessStatusCode);
    }
}
