using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Infrastructure.Adapters.Auth;
using BrahmanGan.Infrastructure.Adapters.EventSourcing;
using BrahmanGan.Infrastructure.Adapters.Messaging;
using BrahmanGan.Infrastructure.Adapters.Persistence;
using BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;
using BrahmanGan.Infrastructure.Common;

namespace BrahmanGan.Infrastructure;

/// <summary>Registro de la capa de infraestructura (EF Core + repositorios ganaderos).</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ===== Selección de proveedor de base de datos (SQL Server / PostgreSQL) =====
        // El proveedor activo se elige por configuración (Database:Provider). Cada proveedor
        // usa su propia cadena de conexión y su propio assembly de migraciones.
        var provider = DatabaseProviderResolver.Resolve(configuration);

        var connectionString = configuration.GetConnectionString(
            DatabaseProviderResolver.ConnectionStringName("DefaultConnection", provider));
        services.AddDbContext<ApplicationDbContext>(options =>
            DatabaseProviderResolver.Configure(options, provider, connectionString));

        var eventStoreConnectionString = configuration.GetConnectionString(
            DatabaseProviderResolver.ConnectionStringName("EventStoreConnection", provider))
            ?? connectionString;
        services.AddDbContext<EventStoreDbContext>(options =>
            DatabaseProviderResolver.Configure(options, provider, eventStoreConnectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // ===== Repositorios ganaderos (Fases 1-6) =====
        // Fase 1
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IRazaRepository, RazaRepository>();
        services.AddScoped<IOrigenRepository, OrigenRepository>();
        services.AddScoped<IHistorialAnimalRepository, HistorialAnimalRepository>();
        services.AddScoped<IMovimientoAnimalRepository, MovimientoAnimalRepository>();
        services.AddScoped<IPesajeRepository, PesajeRepository>();
        services.AddScoped<IMarcacionRepository, MarcacionRepository>();
        // Fase 2
        services.AddScoped<IPaisRepository, PaisRepository>();
        services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        services.AddScoped<IMunicipioRepository, MunicipioRepository>();
        services.AddScoped<IZonaRepository, ZonaRepository>();
        services.AddScoped<IFincaRepository, FincaRepository>();
        services.AddScoped<IPotreroRepository, PotreroRepository>();
        services.AddScoped<IGrupoManejoRepository, GrupoManejoRepository>();
        services.AddScoped<IAnimalPotreroRepository, AnimalPotreroRepository>();
        // Fase 3
        services.AddScoped<ISemenRepository, SemenRepository>();
        services.AddScoped<IPedigriRepository, PedigriRepository>();
        services.AddScoped<IServicioRepository, ServicioRepository>();
        services.AddScoped<IGestacionRepository, GestacionRepository>();
        services.AddScoped<INacimientoRepository, NacimientoRepository>();
        // Fase 4
        services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
        services.AddScoped<IControlPreventivoRepository, ControlPreventivoRepository>();
        services.AddScoped<IHistorialVacunacionRepository, HistorialVacunacionRepository>();
        services.AddScoped<IHistorialPreventivoRepository, HistorialPreventivoRepository>();
        services.AddScoped<IHistorialDesparasitacionRepository, HistorialDesparasitacionRepository>();
        services.AddScoped<IHistorialCurativoRepository, HistorialCurativoRepository>();
        services.AddScoped<IHistorialMastitisRepository, HistorialMastitisRepository>();
        services.AddScoped<IComplementoRepository, ComplementoRepository>();
        // Fase 5
        services.AddScoped<IControlLecheAnimalRepository, ControlLecheAnimalRepository>();
        services.AddScoped<IProduccionLecheRepository, ProduccionLecheRepository>();
        services.AddScoped<IParametroLactanciaRepository, ParametroLactanciaRepository>();
        services.AddScoped<ICalidadLecheRepository, CalidadLecheRepository>();
        services.AddScoped<IVentaLecheRepository, VentaLecheRepository>();
        // Fase 6
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IContratoRepository, ContratoRepository>();
        services.AddScoped<ICotizacionVentaRepository, CotizacionVentaRepository>();

        // Fase 7
        services.AddScoped<ICentroCostoRepository, CentroCostoRepository>();
        // Fase 8
        services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
        services.AddScoped<IPagoJornalRepository, PagoJornalRepository>();
        services.AddScoped<ICapturaCarbonoRepository, CapturaCarbonoRepository>();
        services.AddScoped<IConsumoAguaRepository, ConsumoAguaRepository>();
        services.AddScoped<IEventoMedioambientalRepository, EventoMedioambientalRepository>();
        services.AddScoped<IGastoGeneralRepository, GastoGeneralRepository>();
        services.AddScoped<IIngresoRepository, IngresoRepository>();
        services.AddScoped<IInsumoRepository, InsumoRepository>();
        services.AddScoped<IKardexInsumoRepository, KardexInsumoRepository>();
        services.AddScoped<IAcumulacionInsumoPotreroRepository, AcumulacionInsumoPotreroRepository>();
        services.AddScoped<IMaquinariaRepository, MaquinariaRepository>();
        services.AddScoped<IMantenimientoEquipoRepository, MantenimientoEquipoRepository>();
        services.AddScoped<IRegistroICARepository, RegistroICARepository>();

        // Alimentación
        services.AddScoped<IPlanAlimentacionRepository, PlanAlimentacionRepository>();
        services.AddScoped<IDetallePlanAlimentacionRepository, DetallePlanAlimentacionRepository>();
        // Pastoreo
        services.AddScoped<IPlanPastoreoRepository, PlanPastoreoRepository>();

        // Seguridad
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IPermisoRepository, PermisoRepository>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<IJwtServicio, JwtServicio>();

        return services;
    }
}
