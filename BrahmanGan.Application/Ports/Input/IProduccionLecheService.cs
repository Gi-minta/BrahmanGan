using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IProduccionLecheService
{
    Task<ProduccionLecheResponse> RegistrarAsync(RegistrarProduccionLecheRequest req, CancellationToken ct = default);
}
