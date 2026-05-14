using BrahmanGan.Application;
using BrahmanGan.Infrastructure;
using BrahmanGan.Infrastructure.Adapters.Persistence;
using BrahmanGan.API.Extensions;
using BrahmanGan.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── Controladores ──────────────────────────────────────────────
builder.Services.AddControllers();

// ── Swagger ────────────────────────────────────────────────────
builder.Services.AddSwaggerDocumentation();

// ── Autenticación JWT + OAuth2 (Google) ────────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── Application Layer ──────────────────────────────────────────
builder.Services.AddApplication();

// ── Infrastructure Layer ───────────────────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

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
    app.UseSwaggerDocumentation();
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

app.MapControllers();

// ── Seed inicial (admin + migración) ──────────────────────────
await DbInitializer.InicializarAsync(app.Services);

app.Run();
