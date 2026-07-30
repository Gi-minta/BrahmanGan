using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence;

/// <summary>
/// Inicializa la base de datos con datos esenciales de arranque.
/// Se ejecuta una sola vez si los datos no existen.
///
/// Usuario administrador semilla (configurable, sin credenciales en el código):
///   Seed:Admin:Email    — por defecto admin@brahmangan.com
///   Seed:Admin:Nombre   — por defecto "Administrador del Sistema"
///   Seed:Admin:Password — si no se configura, se genera una contraseña aleatoria y
///                         se registra UNA sola vez en el log de arranque.
///
/// ⚠️ Configura Seed:Admin:Password de forma segura (variable de entorno
///    Seed__Admin__Password, user-secrets o un appsettings NO versionado) y cámbiala
///    tras el primer inicio de sesión.
/// </summary>
public static class DbInitializer
{
    private const string DefaultAdminEmail  = "admin@brahmangan.com";
    private const string DefaultAdminNombre = "Administrador del Sistema";

    public static async Task InicializarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db      = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher  = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var config  = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        var adminEmail  = config["Seed:Admin:Email"]?.Trim()  is { Length: > 0 } e ? e : DefaultAdminEmail;
        var adminNombre = config["Seed:Admin:Nombre"]?.Trim() is { Length: > 0 } n ? n : DefaultAdminNombre;

        var adminPassword    = config["Seed:Admin:Password"];
        var passwordGenerada = string.IsNullOrWhiteSpace(adminPassword);
        if (passwordGenerada)
            adminPassword = GenerarPasswordAleatoria();

        try
        {
            // Aplica migraciones pendientes automáticamente
            await db.Database.MigrateAsync();

            // ── Verificar si ya existe el admin ───────────────────────
            var adminEmailLower = adminEmail.ToLowerInvariant();
            var adminExiste = await db.Usuarios
                .AnyAsync(u => u.Email == adminEmailLower);

            if (adminExiste)
            {
                logger.LogInformation("DbInitializer: usuario admin ya existe, omitiendo seed.");
                return;
            }

            // ── Obtener el rol Administrador (creado por EF seed) ─────
            var rolAdmin = await db.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Administrador");

            if (rolAdmin is null)
            {
                logger.LogWarning("DbInitializer: rol 'Administrador' no encontrado. Ejecuta la migración primero.");
                return;
            }

            // ── Crear usuario administrador ────────────────────────────
            var hash    = hasher.Hashear(adminPassword!);
            var usuario = Usuario.CrearLocal(adminEmail, adminNombre, hash);
            usuario.ConfirmarEmail();

            // Guardar primero para que la BD genere el Id (int identity)
            await db.Usuarios.AddAsync(usuario);
            await db.SaveChangesAsync();

            // Asignar rol después de tener el Id real
            usuario.AsignarRol(rolAdmin);
            await db.SaveChangesAsync();

            if (passwordGenerada)
            {
                // No había contraseña configurada: se registra la generada UNA vez para
                // permitir el primer acceso. Debe cambiarse de inmediato.
                logger.LogWarning(
                    "DbInitializer: usuario admin creado con contraseña ALEATORIA — Email: {Email} | Password: {Password}. " +
                    "Cámbiala tras el primer inicio de sesión y define Seed:Admin:Password para fijarla.",
                    adminEmail, adminPassword);
            }
            else
            {
                // La contraseña vino de configuración: NO se registra en el log.
                logger.LogInformation(
                    "DbInitializer: usuario admin creado — Email: {Email} (contraseña tomada de Seed:Admin:Password).",
                    adminEmail);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DbInitializer: error al inicializar la base de datos.");
            throw;
        }
    }

    /// <summary>Genera una contraseña aleatoria fuerte (bytes criptográficos en base64url).</summary>
    private static string GenerarPasswordAleatoria()
    {
        Span<byte> bytes = stackalloc byte[18];
        RandomNumberGenerator.Fill(bytes);
        var cuerpo = Convert.ToBase64String(bytes)
            .Replace('+', 'A').Replace('/', 'B').TrimEnd('=');
        // Prefijo con mayúscula, dígito y símbolo para cumplir políticas de complejidad.
        return $"Bg9!{cuerpo}";
    }
}
