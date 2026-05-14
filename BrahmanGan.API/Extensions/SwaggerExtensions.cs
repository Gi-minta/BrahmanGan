namespace BrahmanGan.API.Extensions;

/// <summary>
/// Extensiones para servicios de Swagger
/// </summary>
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "BrahmanGan API",
                Version = "v1",
                Description = "API basada en Arquitectura Hexagonal con DDD",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Development Team"
                }
            });

            // Seguridad JWT en Swagger
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Ingresa el token JWT: **Bearer {token}**",
                Name        = "Authorization",
                Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme      = "Bearer"
            });
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Incluir comentarios XML
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "BrahmanGan API V1");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
