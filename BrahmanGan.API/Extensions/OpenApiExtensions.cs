using FastEndpoints.Swagger;
using Scalar.AspNetCore;

namespace BrahmanGan.API.Extensions;

/// <summary>
/// Extensiones para la documentación OpenAPI (NSwag vía FastEndpoints) con Scalar
/// </summary>
public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = true;
            o.ShortSchemaNames    = true;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "v1";
                s.Title        = "BrahmanGan API";
                s.Version      = "v1";
                s.Description  = "API basada en Arquitectura Hexagonal con DDD";
                s.PostProcess = doc =>
                    doc.Info.Contact = new NSwag.OpenApiContact { Name = "Development Team" };
            };
        });

        return services;
    }

    public static WebApplication UseOpenApiDocumentation(this WebApplication app)
    {
        app.UseSwaggerGen();                                // /swagger/v1/swagger.json
        app.MapScalarApiReference(options =>                // /scalar
        {
            options.WithTitle("BrahmanGan API")
                   .WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json")
                   .AddPreferredSecuritySchemes("JWTBearerAuth");
        });

        // La UI de Swagger vivía en la raíz; se mantiene la costumbre
        app.MapGet("/", () => Results.Redirect("/scalar"))
           .ExcludeFromDescription();

        return app;
    }
}
