using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IVentaLecheService
{
    Task<VentaLecheResponse> RegistrarAsync(RegistrarVentaLecheRequest req, CancellationToken ct = default);
}
