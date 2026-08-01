using System.Runtime.CompilerServices;

namespace BrahmanGan.IntegrationTests;

/// <summary>
/// Fija el modo de compatibilidad de fechas de Npgsql (mapea <c>DateTime</c> a
/// <c>timestamp without time zone</c>) en la carga del módulo de pruebas.
///
/// Npgsql lee este switch UNA sola vez, al inicializar su mapeo de tipos, por lo que
/// debe establecerse antes de cualquier uso de Npgsql. En los tests de endpoints se
/// abre una conexión temprana (<see cref="Endpoints.ApiFactory.PostgresDisponible"/>)
/// antes de arrancar la API, así que fijarlo en <c>[ModuleInitializer]</c> garantiza
/// que esté activo antes de la primera conexión, con independencia del orden de los tests.
/// </summary>
internal static class NpgsqlLegacyTimestampInitializer
{
    [ModuleInitializer]
    internal static void Init()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
