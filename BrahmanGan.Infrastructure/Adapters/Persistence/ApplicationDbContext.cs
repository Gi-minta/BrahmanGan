using System.Reflection;
using Microsoft.EntityFrameworkCore;
using BrahmanGan.Domain.Modulos.Inventario;
using BrahmanGan.Domain.Modulos.Finca;
using BrahmanGan.Domain.Modulos.Reproduccion;
using BrahmanGan.Domain.Modulos.Sanidad;
using BrahmanGan.Domain.Modulos.Leche;
using BrahmanGan.Domain.Modulos.Comercial;
using BrahmanGan.Domain.Modulos.Costos;
using BrahmanGan.Domain.Modulos.Almacen;
using BrahmanGan.Domain.Modulos.Nomina;
using BrahmanGan.Domain.Modulos.Trazabilidad;
using BrahmanGan.Domain.Modulos.Equipos;
using BrahmanGan.Domain.Modulos.Sostenibilidad;
using BrahmanGan.Domain.Modulos.Seguridad;
using BrahmanGan.Domain.Modulos.Alimentacion;
using BrahmanGan.Domain.Modulos.Pastoreo;

namespace BrahmanGan.Infrastructure.Adapters.Persistence;

/// <summary>
/// Contexto principal: BrahmanGanBD.
/// Tablas y nombres siguen la convención del script SQL (español).
/// </summary>
public class ApplicationDbContext : DbContext
{
    // ===== Fase 1: Inventario =====
    public DbSet<Animal> Animales => Set<Animal>();
    public DbSet<Raza> Razas => Set<Raza>();
    public DbSet<Origen> Origenes => Set<Origen>();
    public DbSet<HistorialAnimal> HistorialAnimales => Set<HistorialAnimal>();
    public DbSet<MovimientoAnimal> MovimientosAnimales => Set<MovimientoAnimal>();
    public DbSet<Pesaje> Pesajes => Set<Pesaje>();
    public DbSet<Marcacion> Marcaciones => Set<Marcacion>();

    // ===== Fase 2: Finca / Geografía =====
    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Municipio> Municipios => Set<Municipio>();
    public DbSet<Zona> Zonas => Set<Zona>();
    public DbSet<Domain.Modulos.Finca.Finca> Fincas => Set<Domain.Modulos.Finca.Finca>();
    public DbSet<ZonaFinca> ZonaFincas => Set<ZonaFinca>();
    public DbSet<Potrero> Potreros => Set<Potrero>();
    public DbSet<GrupoManejo> GruposManejo => Set<GrupoManejo>();
    public DbSet<AnimalPotrero> AnimalPotreros => Set<AnimalPotrero>();

    // ===== Fase 3: Reproducción =====
    public DbSet<Semen> Semenes => Set<Semen>();
    public DbSet<Pedigri> Pedigries => Set<Pedigri>();
    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<Gestacion> Gestaciones => Set<Gestacion>();
    public DbSet<Nacimiento> Nacimientos => Set<Nacimiento>();

    // ===== Fase 4: Sanidad =====
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<ControlPreventivo> ControlesPreventivos => Set<ControlPreventivo>();
    public DbSet<HistorialPreventivo> HistorialPreventivo => Set<HistorialPreventivo>();
    public DbSet<HistorialVacunacion> HistorialVacunacion => Set<HistorialVacunacion>();
    public DbSet<HistorialDesparasitacion> HistorialDesparasitacion => Set<HistorialDesparasitacion>();
    public DbSet<HistorialCurativo> HistorialCurativo => Set<HistorialCurativo>();
    public DbSet<DetalleCurativo> DetallesCurativos => Set<DetalleCurativo>();
    public DbSet<HistorialMastitis> HistorialMastitis => Set<HistorialMastitis>();
    public DbSet<Complemento> Complementos => Set<Complemento>();

    // ===== Fase 5: Producción de leche =====
    public DbSet<ParametroLactancia> ParametrosLactancia => Set<ParametroLactancia>();
    public DbSet<ControlLecheAnimal> ControlLecheAnimal => Set<ControlLecheAnimal>();
    public DbSet<ProduccionLeche> ProduccionLeche => Set<ProduccionLeche>();
    public DbSet<CalidadLeche> CalidadLeche => Set<CalidadLeche>();
    public DbSet<VentaLeche> VentasLeche => Set<VentaLeche>();

    // ===== Fase 6: Comercial =====
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Contrato> Contratos => Set<Contrato>();
    public DbSet<CotizacionVenta> CotizacionesVenta => Set<CotizacionVenta>();
    public DbSet<DetalleCotizacion> DetalleCotizacion => Set<DetalleCotizacion>();

    // ===== Fase 7: Costos =====
    public DbSet<CentroCosto> CentrosCosto => Set<CentroCosto>();
    public DbSet<Activo> Activos => Set<Activo>();
    public DbSet<CostoDiario> CostosDiarios => Set<CostoDiario>();
    public DbSet<CostoAnimalDiario> CostosAnimalesDiarios => Set<CostoAnimalDiario>();
    public DbSet<GastoGeneral> GastosGenerales => Set<GastoGeneral>();
    public DbSet<Ingreso> Ingresos => Set<Ingreso>();
    public DbSet<Autoconsumo> Autoconsumos => Set<Autoconsumo>();
    public DbSet<TransferenciaCosto> TransferenciasCosto => Set<TransferenciaCosto>();

    // ===== Fase 7: Almacén =====
    public DbSet<Insumo> Insumos => Set<Insumo>();
    public DbSet<KardexInsumo> KardexInsumos => Set<KardexInsumo>();
    public DbSet<AcumulacionInsumoPotrero> AcumulacionInsumosPotrero => Set<AcumulacionInsumoPotrero>();

    // ===== Fase 7: Nómina =====
    public DbSet<Trabajador> Trabajadores => Set<Trabajador>();
    public DbSet<PrestacionSocial> PrestacionesSociales => Set<PrestacionSocial>();
    public DbSet<PagoJornal> PagosJornales => Set<PagoJornal>();

    // ===== Fase 7: Trazabilidad =====
    public DbSet<RegistroICA> RegistrosICA => Set<RegistroICA>();

    // ===== Fase 7: Equipos =====
    public DbSet<Maquinaria> Maquinaria => Set<Maquinaria>();
    public DbSet<MantenimientoEquipo> MantenimientoEquipos => Set<MantenimientoEquipo>();

    // ===== Fase 7: Sostenibilidad =====
    public DbSet<CapturaCarbono> CapturaCarbono => Set<CapturaCarbono>();
    public DbSet<ConsumoAgua> ConsumosAgua => Set<ConsumoAgua>();
    public DbSet<EventoMedioambiental> EventosMedioambientales => Set<EventoMedioambiental>();

    // ===== Alimentación =====
    public DbSet<PlanAlimentacion> PlanesAlimentacion => Set<PlanAlimentacion>();
    public DbSet<DetallePlanAlimentacion> DetallePlanAlimentacion => Set<DetallePlanAlimentacion>();

    // ===== Pastoreo =====
    public DbSet<PlanPastoreo> PlanesPastoreo => Set<PlanPastoreo>();

    // ===== Seguridad (Auth, Roles, Permisos) =====
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<UsuarioRol> UsuariosRoles => Set<UsuarioRol>();
    public DbSet<RolPermiso> RolesPermisos => Set<RolPermiso>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // EF Core uses null as the sentinel for reference-type keys, but every IntId
        // subtype uses IntId(0) as "not yet assigned". This loop tells EF Core that
        // XxxId(0) is the sentinel so it marks those entities as "needs generation"
        // instead of treating 0 as an explicit key value. Without this, adding two
        // new entities in a loop (import) causes a duplicate-key conflict in the
        // identity map before SaveChanges is even called.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var pk = entityType.FindPrimaryKey();
            if (pk is not { Properties.Count: 1 }) continue;

            var pkProp = pk.Properties[0];
            if (!typeof(BrahmanGan.Domain.Common.IntId).IsAssignableFrom(pkProp.ClrType)) continue;

            var fromMethod = pkProp.ClrType.GetMethod(
                "From",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                new[] { typeof(int) });

            if (fromMethod == null) continue;

            var sentinel = fromMethod.Invoke(null, new object[] { 0 });
            modelBuilder.Entity(entityType.ClrType)
                .Property(pkProp.Name)
                .HasSentinel(sentinel);
        }

        base.OnModelCreating(modelBuilder);
    }
}
