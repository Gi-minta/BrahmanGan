using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IVacunacionService
{
    Task<VacunacionResponse> AplicarAsync(AplicarVacunaRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<VacunacionResponse>> ListarPorAnimalAsync(int idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<VacunacionResponse>> ListarAlertasAsync(int diasUmbral = 7, CancellationToken ct = default);
}
