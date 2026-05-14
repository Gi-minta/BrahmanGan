using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IPesajeService
{
    Task<PesajeResponse> RegistrarAsync(RegistrarPesajeRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<PesajeResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default);
}
