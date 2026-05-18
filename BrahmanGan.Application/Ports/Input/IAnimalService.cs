using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

// Fase 1
public interface IAnimalService
{
    Task<AnimalResponse> RegistrarAsync(CrearAnimalRequest req, CancellationToken ct = default);
    Task<AnimalResponse?> ObtenerAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<AnimalResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default);
    Task<IReadOnlyList<AnimalResponse>> ListarActivosAsync(CancellationToken ct = default);
    Task CambiarEstadoAsync(int id, CambiarEstadoAnimalRequest req, CancellationToken ct = default);
    Task TrasladarAsync(int id, TrasladarAnimalRequest req, CancellationToken ct = default);

    Task<IReadOnlyList<HistorialAnimalResponse>> ListarHistorialAsync(int idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<MovimientoAnimalResponse>> ListarMovimientosAsync(int idAnimal, CancellationToken ct = default);

    Task<PedigriResponse> CrearPedigriAsync(CrearPedigriRequest req, CancellationToken ct = default);
    Task<PedigriResponse?> ObtenerPedigriAsync(int idAnimal, CancellationToken ct = default);
}
