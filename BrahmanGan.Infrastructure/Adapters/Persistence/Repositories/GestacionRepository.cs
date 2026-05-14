using Microsoft.EntityFrameworkCore;
using BrahmanGan.Application.Ports.Output;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Reproduccion;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Repositories;

public sealed class GestacionRepository : RepositoryBase<Gestacion, GestacionId>, IGestacionRepository
{
    public GestacionRepository(ApplicationDbContext db) : base(db) { }
    public Task<Gestacion?> GetEnCursoByAnimalAsync(AnimalId idAnimal, CancellationToken ct = default)
        => Set.FirstOrDefaultAsync(g => g.IdAnimal == idAnimal && g.Estado == EstadoGestacion.EN_CURSO, ct);
}
