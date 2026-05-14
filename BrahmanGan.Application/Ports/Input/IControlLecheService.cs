using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 5
public interface IControlLecheService
{
    Task<ControlLecheResponse> RegistrarAsync(RegistrarControlLecheRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<ControlLecheResponse>> ListarPorAnimalAsync(int idAnimal, DateOnly desde, DateOnly hasta, CancellationToken ct = default);
}
