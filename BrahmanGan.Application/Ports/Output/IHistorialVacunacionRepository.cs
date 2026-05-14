using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Application.Ports.Output;

public interface IHistorialVacunacionRepository : IRepository<HistorialVacunacion, HistorialVacunacionId>
{
    Task<IReadOnlyList<HistorialVacunacion>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default);
    Task<IReadOnlyList<HistorialVacunacion>> ListConAlertaAsync(int diasUmbral = 7, CancellationToken ct = default);
}
