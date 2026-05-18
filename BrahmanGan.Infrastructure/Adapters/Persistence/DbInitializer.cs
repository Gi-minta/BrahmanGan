using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Seguridad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence;

/// <summary>
/// Inicializa la base de datos con datos esenciales de arranque.
/// Se ejecuta una sola vez si los datos no existen.
///
/// Usuario administrador por defecto:
///   Email   : admin@brahmangan.com
///   Password: Admin@2025!
///
/// ⚠️ Cambia la contraseña después del primer inicio de sesión.
/// </summary>
public static class DbInitializer
{
    private const string AdminEmail    = "admin@brahmangan.com";
    private const string AdminPassword = "Admin@2025!";
    private const string AdminNombre   = "Administrador del Sistema";

    public static async Task InicializarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db      = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hasher  = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Aplica migraciones pendientes automáticamente
            await db.Database.MigrateAsync();

            // ── Verificar si ya existe el admin ───────────────────────
            var adminExiste = await db.Usuarios
                .AnyAsync(u => u.Email == AdminEmail.ToLowerInvariant());

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
            var hash    = hasher.Hashear(AdminPassword);
            var usuario = Usuario.CrearLocal(AdminEmail, AdminNombre, hash);
            usuario.ConfirmarEmail();

            // Guardar primero para que la BD genere el Id (int identity)
            await db.Usuarios.AddAsync(usuario);
            await db.SaveChangesAsync();

            // Asignar rol después de tener el Id real
            usuario.AsignarRol(rolAdmin);
            await db.SaveChangesAsync();

            logger.LogInformation(
                "DbInitializer: usuario admin creado — Email: {Email} | Password: {Password}",
                AdminEmail, AdminPassword);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DbInitializer: error al inicializar la base de datos.");
            throw;
        }
    }
}
