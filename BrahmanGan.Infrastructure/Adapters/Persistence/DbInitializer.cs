using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Infrastructure.Adapters.EventSourcing;

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
        var db         = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var eventStore = scope.ServiceProvider.GetRequiredService<EventStoreDbContext>();
        var hasher     = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var config     = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger     = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

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

            // El event store tiene su propio DbContext y su propio set de migraciones, y
            // puede apuntar a otra base de datos (EventStoreConnection). Debe migrarse aquí
            // —y antes del return por admin existente— o la tabla DomainEvents nunca se crea.
            await eventStore.Database.MigrateAsync();

            await SembrarPermisosDeRolesAsync(db, logger);

            // ── Verificar si ya existe el admin ───────────────────────
            var adminEmailLower = adminEmail.ToLowerInvariant();
            var adminExistente = await db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == adminEmailLower);

            if (adminExistente is not null)
            {
                // El seed solo actúa sobre un admin nuevo, así que cambiar
                // Seed:Admin:Password no tiene efecto una vez creado. Para no quedarse
                // fuera sin acceso a la base de datos, Seed:Admin:ResetPassword=true
                // fuerza el restablecimiento en el siguiente arranque.
                //
                // Exige una contraseña configurada a propósito: sin esa condición, el
                // flag junto a un Seed:Admin:Password vacío dejaría al admin con una
                // contraseña aleatoria distinta en cada reinicio.
                if (!config.GetValue<bool>("Seed:Admin:ResetPassword"))
                {
                    logger.LogInformation("DbInitializer: usuario admin ya existe, omitiendo seed.");
                    return;
                }

                if (passwordGenerada)
                {
                    logger.LogWarning(
                        "DbInitializer: Seed:Admin:ResetPassword está activo pero no hay " +
                        "Seed:Admin:Password configurada. No se restablece nada: definir " +
                        "la contraseña es obligatorio para no dejar el admin con una " +
                        "aleatoria distinta en cada arranque.");
                    return;
                }

                adminExistente.CambiarPassword(hasher.Hashear(adminPassword!));
                await db.SaveChangesAsync();

                logger.LogWarning(
                    "DbInitializer: contraseña del usuario admin RESTABLECIDA desde " +
                    "Seed:Admin:Password para {Email}. Quita Seed:Admin:ResetPassword " +
                    "(o ponlo en false) para que no vuelva a aplicarse en cada arranque.",
                    adminEmail);
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

    /// <summary>
    /// Da a cada rol de sistema un juego de permisos por defecto acorde a su nombre.
    /// </summary>
    /// <remarks>
    /// El seed de EF solo asigna permisos al rol Administrador, así que el resto se
    /// quedaba a cero. Como los endpoints ahora exigen <c>Modulo:Accion</c>, un usuario
    /// de cualquier otro rol recibiría 403 en todo y la aplicación sería inservible para
    /// él.
    /// <para>
    /// Solo actúa sobre los roles que no tienen ningún permiso: así no pisa los ajustes
    /// que se hagan luego desde el módulo de Seguridad, y repetir el arranque no cambia
    /// nada. Es un punto de partida razonable, no una política definitiva.
    /// </para>
    /// </remarks>
    private static async Task SembrarPermisosDeRolesAsync(
        ApplicationDbContext db,
        ILogger logger)
    {
        // Todo lo operativo del día a día, sin el módulo de Seguridad.
        ModuloSistema[] operativos =
        [
            ModuloSistema.Inventario, ModuloSistema.Finca, ModuloSistema.Reproduccion,
            ModuloSistema.Sanidad, ModuloSistema.Leche, ModuloSistema.Comercial,
            ModuloSistema.Costos, ModuloSistema.Nomina, ModuloSistema.Almacen,
            ModuloSistema.Equipos, ModuloSistema.Trazabilidad, ModuloSistema.Sostenibilidad,
            ModuloSistema.Reportes
        ];

        var porDefecto = new Dictionary<string, (ModuloSistema[] Modulos, AccionPermiso[] Acciones)>
        {
            // Gestiona la operación completa, pero no administra la seguridad.
            ["Gerente"] =
                (operativos,
                 [AccionPermiso.Leer, AccionPermiso.Crear, AccionPermiso.Editar,
                  AccionPermiso.Eliminar, AccionPermiso.Exportar]),

            // Su ámbito es la salud y la reproducción; del resto solo consulta.
            ["Veterinario"] =
                ([ModuloSistema.Sanidad, ModuloSistema.Reproduccion],
                 [AccionPermiso.Leer, AccionPermiso.Crear, AccionPermiso.Editar]),

            // Registra actividad diaria: crea y edita, no borra.
            ["Operador"] =
                ([ModuloSistema.Inventario, ModuloSistema.Finca, ModuloSistema.Leche,
                  ModuloSistema.Sanidad, ModuloSistema.Almacen, ModuloSistema.Equipos],
                 [AccionPermiso.Leer, AccionPermiso.Crear, AccionPermiso.Editar]),

            // Solo lectura y exportación, en todo.
            ["Auditor"] =
                (operativos, [AccionPermiso.Leer, AccionPermiso.Exportar])
        };

        var roles = await db.Roles
            .Include(r => r.RolesPermiso)
            .Where(r => porDefecto.Keys.Contains(r.Nombre))
            .ToListAsync();

        var permisos = await db.Permisos.ToListAsync();
        var asignados = 0;

        foreach (var rol in roles)
        {
            if (rol.RolesPermiso.Count > 0)
                continue;

            var (modulos, acciones) = porDefecto[rol.Nombre];
            var aAsignar = permisos.Where(p => modulos.Contains(p.Modulo) && acciones.Contains(p.Accion));

            foreach (var permiso in aAsignar)
            {
                rol.AsignarPermiso(permiso);
                asignados++;
            }

            logger.LogInformation(
                "DbInitializer: rol '{Rol}' no tenía permisos; se le asignan los de partida.",
                rol.Nombre);
        }

        if (asignados > 0)
            await db.SaveChangesAsync();
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
