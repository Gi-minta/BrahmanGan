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
        // Fase 5
        services.AddScoped<IControlLecheService, ControlLecheService>();
        services.AddScoped<IProduccionLecheService, ProduccionLecheService>();
        services.AddScoped<IVentaLecheService, VentaLecheService>();
        // Fase 6
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IContratoService, ContratoService>();
        services.AddScoped<ICotizacionVentaService, CotizacionVentaService>();

        // Seguridad
        services.AddScoped<IAuthServicio, AuthServicio>();
        services.AddScoped<IRolServicio, RolServicio>();
        services.AddScoped<IUsuarioAdminServicio, UsuarioAdminServicio>();

        // Validators FluentValidation
        services.AddValidatorsFromAssemblyContaining<Validators.CrearAnimalRequestValidator>();
        return services;
    }
}
