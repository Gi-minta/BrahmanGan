using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 3
public interface IServicioReproductivoService
{
    Task<ServicioResponse> RegistrarMontaAsync(RegistrarMontaRequest req, CancellationToken ct = default);
    Task<ServicioResponse> RegistrarIaAsync(RegistrarIaRequest req, CancellationToken ct = default);
    Task ConfirmarAsync(int id, ConfirmarServicioRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ServicioResponse>> ListarPorHembraAsync(int idHembra, CancellationToken ct = default);

    Task<SemenResponse> CrearSemenAsync(CrearSemenRequest req, CancellationToken ct = default);
    Task<SemenResponse?> ObtenerSemenAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SemenResponse>> ListarSemenAsync(CancellationToken ct = default);
    Task<SemenResponse> AjustarStockSemenAsync(AjustarStockSemenRequest req, CancellationToken ct = default);

    Task<NacimientoResponse?> ObtenerNacimientoAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<NacimientoResponse>> ListarNacimientosPorGestacionAsync(int idGestacion, CancellationToken ct = default);
}
