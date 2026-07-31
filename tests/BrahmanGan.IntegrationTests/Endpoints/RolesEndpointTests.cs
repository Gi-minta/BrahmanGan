using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace BrahmanGan.IntegrationTests.Endpoints;

public class RolesEndpointTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public RolesEndpointTests(ApiFactory factory) => _factory = factory;

    private static async Task<string> LoginAdminAsync(HttpClient client)
    {
        var resp = await client.PostAsJsonAsync("/api/auth/login",
            new { Email = ApiFactory.AdminEmail, Password = ApiFactory.AdminPassword });
        resp.EnsureSuccessStatusCode();
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("accessToken").GetString()!;
    }

    [SkippableFact]
    public async Task Get_roles_sin_token_devuelve_401()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/api/roles");

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [SkippableFact]
    public async Task Get_roles_con_token_admin_devuelve_los_roles_sembrados()
    {
        Skip.IfNot(ApiFactory.PostgresDisponible(), "PostgreSQL no disponible.");
        var client = _factory.CreateClient();
        var token = await LoginAdminAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var resp = await client.GetAsync("/api/roles");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
        Assert.True(doc.RootElement.GetArrayLength() >= 5); // 5 roles de sistema sembrados
    }
}
