using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Ports.Input;

public interface IPotreroService
{
    Task<PotreroResponse> CrearAsync(CrearPotreroRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<PotreroResponse>> ListarPorFincaAsync(int idFinca, CancellationToken ct = default);

    Task<GrupoManejoResponse> CrearGrupoAsync(CrearGrupoManejoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<GrupoManejoResponse>> ListarGruposAsync(CancellationToken ct = default);

    Task<AnimalPotreroResponse> AsignarAnimalAsync(AsignarAnimalPotreroRequest req, CancellationToken ct = default);
    Task<AnimalPotreroResponse> CerrarAsignacionAsync(int id, CerrarAnimalPotreroRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<AnimalPotreroResponse>> ListarAnimalesPorPotreroAsync(int idPotrero, CancellationToken ct = default);

    Task<AcumulacionInsumoPotreroResponse> RegistrarAcumulacionAsync(RegistrarAcumulacionInsumoRequest req, CancellationToken ct = default);
    Task<IReadOnlyList<AcumulacionInsumoPotreroResponse>> ListarAcumulacionesPorPotreroAsync(int idPotrero, CancellationToken ct = default);
}
