using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using BrahmanGan.Application.Ports.Input;
using BrahmanGan.Application.UseCases.Animales;
using BrahmanGan.Application.UseCases.Fincas;
using BrahmanGan.Application.UseCases.Reproduccion;
using BrahmanGan.Application.UseCases.Sanidad;
using BrahmanGan.Application.UseCases.Leche;
using BrahmanGan.Application.UseCases.Comercial;
using BrahmanGan.Application.UseCases.Auth;
using BrahmanGan.Application.UseCases.Costos;
using BrahmanGan.Application.UseCases.Almacen;
using BrahmanGan.Application.UseCases.Equipos;
using BrahmanGan.Application.UseCases.Trazabilidad;
using BrahmanGan.Application.UseCases.Nomina;
using BrahmanGan.Application.UseCases.Sostenibilidad;
using BrahmanGan.Application.UseCases.Alimentacion;
using BrahmanGan.Application.UseCases.Pastoreo;
using BrahmanGan.Application.ImportacionMasiva.Servicios;

namespace BrahmanGan.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Fase 1
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IRazaService, RazaService>();
        services.AddScoped<IPesajeService, PesajeService>();
        // Fase 2
        services.AddScoped<IFincaService, FincaService>();
        services.AddScoped<IPotreroService, PotreroService>();
        // Fase 3
        services.AddScoped<IServicioReproductivoService, ServicioReproductivoService>();
        services.AddScoped<IGestacionService, GestacionService>();
        // Fase 4
        services.AddScoped<IMedicamentoService, MedicamentoService>();
        services.AddScoped<IVacunacionService, VacunacionService>();
        services.AddScoped<IComplementoService, ComplementoService>();
        // Fase 5
        services.AddScoped<IControlLecheService, ControlLecheService>();
        services.AddScoped<IProduccionLecheService, ProduccionLecheService>();
        services.AddScoped<IVentaLecheService, VentaLecheService>();
        // Fase 6
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IContratoService, ContratoService>();
        services.AddScoped<ICotizacionVentaService, CotizacionVentaService>();

        // Fase 7
        services.AddScoped<ICentroCostoService, CentroCostoService>();
        // Fase 8
        services.AddScoped<ITrabajadorService, TrabajadorService>();
        services.AddScoped<ISostenibilidadService, SostenibilidadService>();
        services.AddScoped<IGastoGeneralService, GastoGeneralService>();
        services.AddScoped<IIngresoService, IngresoService>();
        services.AddScoped<IInsumoService, InsumoService>();
        services.AddScoped<IMaquinariaService, MaquinariaService>();
        services.AddScoped<IRegistroICAService, RegistroICAService>();

        // Alimentación y Pastoreo
        services.AddScoped<IAlimentacionService, AlimentacionService>();
        services.AddScoped<IPastoreoService, PastoreoService>();

        // Seguridad
        services.AddScoped<IAuthServicio, AuthServicio>();
        services.AddScoped<IRolServicio, RolServicio>();
        services.AddScoped<IUsuarioAdminServicio, UsuarioAdminServicio>();

        // Validators FluentValidation
        services.AddValidatorsFromAssemblyContaining<Validators.CrearAnimalRequestValidator>();

        // Importación masiva
        services.AddScoped<ImportRazasService>();
        services.AddScoped<ImportAnimalesService>();
        services.AddScoped<ImportPesajesService>();
        services.AddScoped<ImportFincasService>();
        services.AddScoped<ImportPotrerosService>();
        services.AddScoped<ImportServiciosReproductivosService>();
        services.AddScoped<ImportGestacionesService>();
        services.AddScoped<ImportMedicamentosService>();
        services.AddScoped<ImportVacunacionesService>();
        services.AddScoped<ImportControlLecheService>();
        services.AddScoped<ImportProduccionLecheService>();
        services.AddScoped<ImportVentasLecheService>();
        services.AddScoped<ImportClientesService>();
        services.AddScoped<ImportCentrosCostoService>();
        services.AddScoped<ImportGastosService>();
        services.AddScoped<ImportIngresosService>();
        services.AddScoped<ImportInsumosService>();
        services.AddScoped<ImportMaquinariaService>();
        services.AddScoped<ImportTrabajadoresService>();
        services.AddScoped<ImportPagosJornalService>();
        services.AddScoped<ImportRegistrosICAService>();
        services.AddScoped<ImportCapturaCarbonoService>();
        services.AddScoped<ImportConsumoAguaService>();
        services.AddScoped<ImportPlanesAlimentacionService>();
        services.AddScoped<ImportDetallesAlimentacionService>();
        services.AddScoped<ImportPlanesPastoreoService>();
        services.AddScoped<ImportComplementosService>();
        services.AddScoped<ImportDesparasitacionesService>();
        services.AddScoped<ImportControlesPreventivosService>();
        services.AddScoped<ImportLactanciasService>();
        services.AddScoped<ImportCalidadLecheService>();
        services.AddScoped<ImportSemenService>();

        return services;
    }
}
