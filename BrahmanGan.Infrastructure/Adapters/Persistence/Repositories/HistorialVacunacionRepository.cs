using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sanidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class HistorialVacunacionRepository : RepositoryBase<HistorialVacunacion, HistorialVacunacionId>, IHistorialVacunacionRepository
{
    public HistorialVacunacionRepository(ApplicationDbContext db) : base(db) { }
    public async Task<IReadOnlyList<HistorialVacunacion>> ListByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => await Set.AsNoTracking().Where(h => h.IdAnimal == idAnimal).OrderByDescending(h => h.Fecha).ToListAsync(ct);
    public async Task<IReadOnlyList<HistorialVacunacion>> ListConAlertaAsync(int diasUmbral = 7, CancellationToken ct = default)
    {
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var limite = hoy.AddDays(diasUmbral);
        return await Set.AsNoTracking()
            .Where(h => h.ProximaFecha != null && h.ProximaFecha <= limite)
            .ToListAsync(ct);
    }
}
