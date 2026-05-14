using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface ICotizacionVentaService
{
    Task<CotizacionResponse> CrearAsync(CrearCotizacionRequest req, CancellationToken ct = default);
    Task AgregarDetalleAsync(int idCotizacion, AgregarDetalleCotizacionRequest req, CancellationToken ct = default);
    Task AprobarAsync(int idCotizacion, CancellationToken ct = default);
    Task RechazarAsync(int idCotizacion, CancellationToken ct = default);
    Task<CotizacionResponse?> ObtenerAsync(int idCotizacion, CancellationToken ct = default);
}
