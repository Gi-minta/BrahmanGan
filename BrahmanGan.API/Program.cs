using FastEndpoints;
using BrahmanGan.Application;
using BrahmanGan.Infrastructure;
using BrahmanGan.Infrastructure.Adapters.Persistence;
using BrahmanGan.API.Extensions;
using BrahmanGan.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── FastEndpoints ──────────────────────────────────────────────
builder.Services.AddFastEndpoints();

// ── OpenAPI + Scalar ───────────────────────────────────────────
builder.Services.AddOpenApiDocumentation();

// ── Clave de firma JWT (obligatoria; efímera solo en Development) ──
BrahmanGan.API.Extensions.JwtKeyBootstrap.EnsureJwtSecretKey(builder.Configuration, builder.Environment);

// ── Autenticación JWT + OAuth2 (Google) ────────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── Application Layer ──────────────────────────────────────────
builder.Services.AddApplication();

// ── Infrastructure Layer ───────────────────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── Health checks ──────────────────────────────────────────────
// /health incluye un chequeo de conectividad a la base de datos (agnóstico del
// proveedor: SQL Server o PostgreSQL). /health/live es solo liveness (no toca la BD).
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

// ── CORS — permite el cliente React en desarrollo ──────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
    // Abierto en otros entornos; restringir en producción
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ── Build ──────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Frontend");
}
else
{
    app.UseCors("AllowAll");
}

app.UseHttpsRedirection();

// ── Middleware de excepciones ──────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ── Auth pipeline (ORDEN IMPORTANTE) ─────────────────────────
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Serializer.Options.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter());

    // Los permisos viajan en el claim "permiso" (lo emite AuthServicio con la clave
    // "Modulo:Accion" de Permiso.Clave). FastEndpoints busca "permissions" por defecto,
    // así que sin esto Permissions(...) no encontraría ninguno y todo daría 403.
    c.Security.PermissionsClaimType = "permiso";
});

// La documentación (Scalar en /scalar y el JSON en /swagger/v1/swagger.json) se publica
// siempre en Development. Fuera de Development hay que habilitarla explícitamente con
// OpenApi:Enabled — normalmente la variable de entorno OpenApi__Enabled=true.
//
// Se gobierna por configuración y no por el entorno para no obligar a arrancar producción
// como Development, que además de los errores detallados haría que JwtKeyBootstrap genere
// una clave de firma EFÍMERA y todos los tokens se invaliden en cada reinicio.
//
// Expone el catálogo completo de endpoints y sus esquemas, así que en un despliegue público
// es una decisión deliberada: no exige autenticación para leerse (los endpoints sí la siguen
// exigiendo). El valor por defecto es false.
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("OpenApi:Enabled"))
{
    app.UseOpenApiDocumentation();
}

// ── Health checks ──────────────────────────────────────────────
// /health      → estado general (incluye conectividad a la BD).
// /health/live → liveness (sin dependencias externas).
app.MapHealthChecks("/health").AllowAnonymous();
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false
}).AllowAnonymous();

// ── Seed inicial (admin + migración) ──────────────────────────
await DbInitializer.InicializarAsync(app.Services);

app.Run();

// Hace la clase Program accesible para las pruebas de endpoints (WebApplicationFactory<Program>).
public partial class Program { }
